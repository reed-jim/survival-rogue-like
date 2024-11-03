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

    #region ACTION
    public static event Action<string> hostStartedEvent;
    #endregion

    private void Awake()
    {
        networkManager.OnClientConnectedCallback += HandleOnClientConnected;
    }

    private void OnDestroy()
    {
        networkManager.OnClientConnectedCallback -= HandleOnClientConnected;
    }

    public async void StartHostWithRelay(int maxConnections = 5)
    {
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);

        networkManager.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));

        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        bool isHostStarted = networkManager.StartHost();

        if (isHostStarted)
        {
            hostStartedEvent?.Invoke(joinCode);
        }
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
}
