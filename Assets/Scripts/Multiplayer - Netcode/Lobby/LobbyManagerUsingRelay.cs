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

public class LobbyManagerUsingRelay : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;

    #region PRIVATE FIELD
    private Dictionary<string, string> lobbyIdWithJoinCodeDictionary;
    #endregion

    #region ACTION
    public static event Action<string, string> setJoinCodeEvent;
    public static event Action toGameplayEvent;
    #endregion

    private void Awake()
    {
        LobbyDetailScreen.startGameForLobbyEvent += StartGameForLobby;

        networkManager.OnClientConnectedCallback += HandleOnClientConnected;
        networkManager.OnServerStarted += OnHostStarted;

        lobbyIdWithJoinCodeDictionary = new Dictionary<string, string>();
    }

    private void OnDestroy()
    {
        LobbyDetailScreen.startGameForLobbyEvent -= StartGameForLobby;

        networkManager.OnClientConnectedCallback -= HandleOnClientConnected;
        networkManager.OnServerStarted -= OnHostStarted;
    }

    private void StartGameForLobby(string lobbyId)
    {
        StartHostWithRelay(lobbyId);
    }

    public async void StartHostWithRelay(string lobbyId, int maxConnections = 5)
    {
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);

        networkManager.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));

        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        lobbyIdWithJoinCodeDictionary.Add(lobbyId, joinCode);

        setJoinCodeEvent?.Invoke(lobbyId, joinCode);

        networkManager.StartHost();
    }

    public async Task<bool> StartClientWithRelay(string joinCode)
    {
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode: joinCode);

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));

        return !string.IsNullOrEmpty(joinCode) && NetworkManager.Singleton.StartClient();
    }

    private void HandleOnClientConnected(ulong clientId)
    {

    }

    private void OnHostStarted()
    {
        toGameplayEvent?.Invoke();
    }
}
