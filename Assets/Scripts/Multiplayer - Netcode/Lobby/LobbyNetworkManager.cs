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
    #region PRIVATE FIELD
    private Lobby _currentLobby;
    private string _currentLobbyId;
    #endregion

    #region ACTION
    public static event Action<int, string> updateLobbyRoomItemEvent;
    public static event Action<string> setLobbyId;
    public static event Action<string> switchRoute;
    public static event Action<string> startGameEvent;
    #endregion

    private void Awake()
    {
        LobbyRoomUI.joinLobbyEvent += JoinLobbyByIdAsync;
        LobbyScreen.refreshLobbyListEvent += QueryLobbyAsync;
        LobbyManagerUsingRelay.setJoinCodeEvent += sendJoinCodeAcrossLobby;

        Init();

        SaferioTween.DelayAsync(1, onCompletedAction: () => QueryLobbyAsync());
    }

    private void OnDestroy()
    {
        LobbyRoomUI.joinLobbyEvent -= JoinLobbyByIdAsync;
        LobbyScreen.refreshLobbyListEvent -= QueryLobbyAsync;
        LobbyManagerUsingRelay.setJoinCodeEvent -= sendJoinCodeAcrossLobby;
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
        options.Data = new Dictionary<string, DataObject>();

        _currentLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);

        setLobbyId?.Invoke(_currentLobby.Id);

        _currentLobbyId = _currentLobby.Id;

        OnLobbyCreated();
    }

    private async void OnLobbyCreated()
    {
        switchRoute?.Invoke("LobbyDetail");

        // await QueryLobbyAsync();
    }

    private async void QueryLobbyAsync()
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

            Debug.Log(lobbies.Results.Count);

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

    private async void JoinLobbyByIdAsync(string lobbyId, string joinCode)
    {
        try
        {
            Lobby joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);

            Debug.Log(joinedLobby);
            Debug.Log(joinedLobby.Players.Count);

            if (joinedLobby.Data != null)
            {
                joinCode = joinedLobby.Data["join_code"].Value;
                Debug.Log(joinedLobby.Data["join_code"]);
                foreach (var item in joinedLobby.Data.Values)
                {
                    Debug.Log(item.Value);
                }

                startGameEvent?.Invoke(joinCode);
            }

            var callbacks = new LobbyEventCallbacks();

            callbacks.DataChanged += HandleOnJoinCodeReceived;

            // Debug.Log(joinedLobby);
            // Debug.Log(joinedLobby.Data);

            // if (joinedLobby.Data.TryGetValue("join_code", out DataObject customData))
            // {
            //     string receivedString = customData.Value;

            //     Debug.Log(receivedString);
            // }

            // startGameEvent?.Invoke(joinCode);
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

    private async Task UpdateLobbyAsync(Dictionary<string, DataObject> data)
    {
        var updateOptions = new UpdateLobbyOptions
        {
            Data = data
        };

        await LobbyService.Instance.UpdateLobbyAsync(_currentLobbyId, updateOptions);
    }


    private void HandleOnJoinCodeReceived(Dictionary<string, ChangedOrRemovedLobbyValue<DataObject>> data)
    {
        if (data.ContainsKey("join_code"))
        {
            string joinCode = data["join_code"].Value.Value;

            Debug.Log(data["join_code"].Value);
            Debug.Log(joinCode);

            startGameEvent?.Invoke(joinCode);
        }
    }

    private void sendJoinCodeAcrossLobby(string lobbyId, string joinCode)
    {
        Dictionary<string, DataObject> data = new Dictionary<string, DataObject>
        {
            {
                "join_code",
                new DataObject(
                visibility: DataObject.VisibilityOptions.Public,
                value: joinCode)
            }
        };

        UpdateLobbyAsync(data);
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
