using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyRoomUI : MonoBehaviour, ISaferioPageViewSlot
{
    [SerializeField] private TMP_Text joinCodeText;

    private int _roomIndex;

    private void Awake()
    {
        LobbyScreen.updateLobbyRoomEvent += OnRoomCreated;
    }

    private void OnDestroy()
    {
        LobbyScreen.updateLobbyRoomEvent -= OnRoomCreated;
    }

    public void Setup(int slotIndex)
    {
        _roomIndex = slotIndex;
    }

    private void OnRoomCreated(int roomIndex, string joinCode)
    {
        if (roomIndex == _roomIndex)
        {
            joinCodeText.text = joinCode;
        }
    }
}
