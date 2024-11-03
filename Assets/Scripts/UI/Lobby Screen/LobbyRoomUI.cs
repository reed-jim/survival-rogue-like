using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyRoomUI : MonoBehaviour, ISaferioPageViewSlot
{
    [SerializeField] private TMP_Text lobbyIdText;
    [SerializeField] private TMP_Text joinCodeText;

    [SerializeField] private Button joinButton;

    private int _lobbyIndex;
    private string _lobbyId;
    private string _joinCode;

    #region ACTION
    public static event Action<string> joinLobbyEvent;
    #endregion

    private void Awake()
    {
        // LobbyScreen.updateLobbyRoomEvent += OnRoomCreated;
        LobbyNetworkManager.updateLobbyRoomItemEvent += OnLobbyCreated;
        LobbyManagerUsingRelay.setJoinCodeEvent += SetJoinCode;

        joinButton.onClick.AddListener(JoinLobby);
    }

    private void OnDestroy()
    {
        // LobbyScreen.updateLobbyRoomEvent -= OnRoomCreated;
        LobbyNetworkManager.updateLobbyRoomItemEvent -= OnLobbyCreated;
        LobbyManagerUsingRelay.setJoinCodeEvent -= SetJoinCode;
    }

    public void Setup(int slotIndex)
    {
        _lobbyIndex = slotIndex;

        gameObject.SetActive(false);
    }

    private void OnLobbyCreated(int roomIndex, string lobbyId)
    {
        if (roomIndex == _lobbyIndex)
        {
            lobbyIdText.text = lobbyId;

            _lobbyId = lobbyId;

            gameObject.SetActive(true);
        }
    }

    private void SetJoinCode(string lobbyId, string joinCode)
    {
        if (lobbyId == _lobbyId)
        {
            _joinCode = joinCode;

            joinCodeText.text = $"{joinCode}";
        }
    }

    private void JoinLobby()
    {
        joinLobbyEvent?.Invoke(_lobbyId);
    }
}
