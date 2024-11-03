using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyScreen : UIScreen
{
    [SerializeField] private LobbyManagerUsingRelay lobbyManagerUsingRelay;

    [SerializeField] private Button createRoomButton;
    [SerializeField] private Button joinRoomButton;

    public override void RegisterEvent()
    {
        base.RegisterEvent();

        createRoomButton.onClick.AddListener(CreateRoom);
        joinRoomButton.onClick.AddListener(JoinRoom);
    }

    private void CreateRoom()
    {
        lobbyManagerUsingRelay.StartHostWithRelay();
    }

    private void JoinRoom()
    {
        // lobbyNetworkManager.JoinRoom();
    }
}
