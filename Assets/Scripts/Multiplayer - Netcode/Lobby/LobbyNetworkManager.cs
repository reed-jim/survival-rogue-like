using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Saferio.Util.SaferioTween;
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
    #region ACTION
    public static event Action<int, string> updateLobbyRoomItemEvent;
    public static event Action<string> setLobbyId;
    public static event Action<string> switchRoute;
    #endregion

    private void Awake()
    {
        LobbyRoomUI.joinLobbyEvent += JoinLobbyByIdAsync;

        Init();

        SaferioTween.DelayAsync(1, onCompletedAction: () => QueryLobbyAsync());
    }

    private void OnDestroy()
    {
        LobbyRoomUI.joinLobbyEvent -= JoinLobbyByIdAsync;
    }

    public string GenerateString(int length = 10, string chars = "abcdefghijklmnopqrstuvwxyz")
    {
        char[] stringChars = new char[length];
        System.Random random = new System.Random();

        for (int i = 0; i < length; i++)
        {
            int index = random.Next(chars.Length);
            stringChars[i] = chars[index];
        }

        return new string(stringChars);
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

    public async void CreatePublicLobbyAsync()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized || !AuthenticationService.Instance.IsAuthorized)
        {
            return;
        }

        string lobbyName = GenerateString();
        int maxPlayers = 4;
        CreateLobbyOptions options = new CreateLobbyOptions();
        options.IsPrivate = false;

        Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);

        setLobbyId?.Invoke(lobby.Id);

        OnLobbyCreated();
    }

    private async void OnLobbyCreated()
    {
        switchRoute?.Invoke("LobbyDetail");

        // await QueryLobbyAsync();
    }

    private async Task QueryLobbyAsync()
    {
        try
        {
            QueryLobbiesOptions options = new QueryLobbiesOptions();
            options.Count = 25;

            // // Filter for open lobbies only
            // options.Filters = new List<QueryFilter>()
            // {
            //     new QueryFilter(
            //         field: QueryFilter.FieldOptions.AvailableSlots,
            //         op: QueryFilter.OpOptions.GT,
            //         value: "0")
            // };

            // // Order by newest lobbies first
            // options.Order = new List<QueryOrder>()
            // {
            //     new QueryOrder(
            //         asc: false,
            //         field: QueryOrder.FieldOptions.Created)
            // };

            QueryResponse lobbies = await LobbyService.Instance.QueryLobbiesAsync(options);

            for (int i = 0; i < lobbies.Results.Count; i++)
            {
                updateLobbyRoomItemEvent?.Invoke(i, lobbies.Results[i].Id);
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void JoinLobbyByIdAsync(string lobbyId)
    {
        try
        {
            Lobby joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async Task JoinLobbyByCodeAysnc(string lobbyCode)
    {
        try
        {
            Lobby joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);
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
