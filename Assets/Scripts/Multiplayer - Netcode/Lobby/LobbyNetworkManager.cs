using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class LobbyNetworkManager : MonoBehaviour
{
    private void Awake()
    {
        Init();
    }

    private async void Init()
    {
        await InitializeLobbyAPI();

        Authenticate();
    }

    private async Task InitializeLobbyAPI()
    {
        await UnityServices.InitializeAsync();
    }

    private async void Authenticate()
    {
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async Task<string> StartHostWithRelay(int maxConnections = 5)
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));
        var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        return NetworkManager.Singleton.StartHost() ? joinCode : null;
    }

    public async void CreatePublicLobbyAsync()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized || !AuthenticationService.Instance.IsAuthorized)
        {
            return;
        }

        string lobbyName = "new lobby";
        int maxPlayers = 4;
        CreateLobbyOptions options = new CreateLobbyOptions();
        options.IsPrivate = false;

        Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);

        Debug.Log(lobby);

        OnLobbyCreated();
    }

    private async void OnLobbyCreated()
    {
        await QueryLobbyAsync();
    }

    private async Task QueryLobbyAsync()
    {
        try
        {
            QueryLobbiesOptions options = new QueryLobbiesOptions();
            options.Count = 25;

            // Filter for open lobbies only
            options.Filters = new List<QueryFilter>()
            {
                new QueryFilter(
                    field: QueryFilter.FieldOptions.AvailableSlots,
                    op: QueryFilter.OpOptions.GT,
                    value: "0")
            };

            // Order by newest lobbies first
            options.Order = new List<QueryOrder>()
            {
                new QueryOrder(
                    asc: false,
                    field: QueryOrder.FieldOptions.Created)
            };

            QueryResponse lobbies = await LobbyService.Instance.QueryLobbiesAsync(options);

            foreach (var lobby in lobbies.Results)
            {
                DebugUtil.DistinctLog(lobby);
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }







    // [SerializeField] private NetworkManager networkManager;

    // #region ACTION
    // public static event Action createRoomEvent;
    // #endregion

    // private void Awake()
    // {
    //     networkManager.OnServerStarted += HandleOnServerStarted;
    //     networkManager.OnClientConnectedCallback += HandleOnClientConnected;
    // }

    // private void OnDestroy()
    // {
    //     networkManager.OnServerStarted -= HandleOnServerStarted;
    //     networkManager.OnClientConnectedCallback -= HandleOnClientConnected;
    // }

    // public void CreateRoom()
    // {
    //     networkManager.StartHost();
    //     networkManager.StartServer();
    // }

    // public void JoinRoom()
    // {
    //     networkManager.StartClient();
    // }

    // private void HandleOnServerStarted()
    // {
    //     createRoomEvent?.Invoke();
    // }

    // private void HandleOnClientConnected(ulong clientId)
    // {

    // }
}
