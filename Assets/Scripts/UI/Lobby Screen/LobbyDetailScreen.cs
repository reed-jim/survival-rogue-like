using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyDetailScreen : UIScreen
{
    [SerializeField] private TMP_Text lobbyIdText;
    [SerializeField] private TMP_Text joinCodeText;
    [SerializeField] private Button startGameButton;

    private string _lobbyId;
    private string _joinCode;

    #region ACTION
    public static event Action<string> startGameForLobbyEvent;
    #endregion

    public override void RegisterEvent()
    {
        base.RegisterEvent();

        LobbyNetworkManager.setLobbyId += SetLobbyId;
        LobbyManagerUsingRelay.setJoinCodeEvent += SetJoinCode;

        startGameButton.onClick.AddListener(StartGame);
    }

    public override void UnregisterEvent()
    {
        base.UnregisterEvent();

        LobbyNetworkManager.setLobbyId -= SetLobbyId;
        LobbyManagerUsingRelay.setJoinCodeEvent -= SetJoinCode;
    }

    private void SetLobbyId(string lobbyId)
    {
        _lobbyId = lobbyId;

        lobbyIdText.text = $"Lobby Id: {_lobbyId}";
    }

    private void SetJoinCode(string lobbyId, string joinCode)
    {
        if (lobbyId == _lobbyId)
        {
            _joinCode = joinCode;

            joinCodeText.text = $"Join Code: {_joinCode}";
        }
    }

    private void StartGame()
    {
        startGameForLobbyEvent?.Invoke(_lobbyId);
    }
}
