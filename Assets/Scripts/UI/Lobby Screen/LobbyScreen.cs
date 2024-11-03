using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyScreen : UIScreen
{
    [SerializeField] private LobbyNetworkManager lobbyNetworkManager;

    [SerializeField] private Button createRoomButton;
    [SerializeField] private Button joinRoomButton;

    #region PRIVATE FIELD
    private int _currentRoomIndex;
    #endregion

    #region ACTION
    public static event Action<int, string> updateLobbyRoomEvent;
    #endregion

    public override void RegisterEvent()
    {
        base.RegisterEvent();

        createRoomButton.onClick.AddListener(CreateRoom);
        joinRoomButton.onClick.AddListener(JoinRoom);
    }

    public override void UnregisterEvent()
    {

    }

    private void CreateRoom()
    {
        lobbyNetworkManager.CreatePublicLobbyAsync();
    }

    private void JoinRoom()
    {
        // lobbyNetworkManager.JoinRoom();
    }

    // private void OnHostStarted(string joinCode)
    // {
    //     updateLobbyRoomEvent?.Invoke(_currentRoomIndex, joinCode);

    //     _currentRoomIndex++;
    // }
}
