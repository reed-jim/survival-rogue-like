using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonLaucher : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        Connect();
    }

    private void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {

        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    #region MonoBehaviourPunCallbacks Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        // RoomOptions roomOptions = new RoomOptions { MaxPlayers = 2, IsVisible = true };

        // PhotonNetwork.CreateRoom("Room 1");
    }

    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room Created");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room Created Failed: " + message);
    }
    #endregion
}
