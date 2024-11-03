using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyScreen : UIScreen
{
    [SerializeField] private LobbyManagerUsingRelay lobbyManagerUsingRelay;

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

        LobbyManagerUsingRelay.hostStartedEvent += OnHostStarted;

        createRoomButton.onClick.AddListener(CreateRoom);
        joinRoomButton.onClick.AddListener(JoinRoom);
    }

    private void OnDestroy()
    {
        LobbyManagerUsingRelay.hostStartedEvent -= OnHostStarted;
    }

    private void CreateRoom()
    {
        lobbyManagerUsingRelay.StartHostWithRelay();
    }

    private void JoinRoom()
    {
        // lobbyNetworkManager.JoinRoom();
    }

    private void OnHostStarted(string joinCode)
    {
        updateLobbyRoomEvent?.Invoke(_currentRoomIndex, joinCode);

        _currentRoomIndex++;
    }
}
