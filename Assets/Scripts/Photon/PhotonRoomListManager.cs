using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhotonRoomListManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private RectTransform roomPrefab;
    [SerializeField] private Button roomButton;
    [SerializeField] private Button createRoomButton;

    [SerializeField] private TMP_Text roomNames;

    private void Awake()
    {
        createRoomButton.onClick.AddListener(CreateRoom);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var item in roomList)
        {
            roomNames.text = $"{item.Name}  -  {item.PlayerCount}/{item.MaxPlayers}";

            roomButton.onClick.RemoveAllListeners();
            roomButton.onClick.AddListener(JoinRoom);
        }

        roomPrefab.gameObject.SetActive(true);
    }

    private void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    private void CreateRoom()
    {
        PhotonNetwork.CreateRoom("Room 1");
    }
}
