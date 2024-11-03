using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyScreen : UIScreen
{
    [SerializeField] private LobbyNetworkManager lobbyNetworkManager;

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
        lobbyNetworkManager.CreatePublicLobbyAsync();
    }

    private void JoinRoom()
    {
        // lobbyNetworkManager.JoinRoom();
    }
}
