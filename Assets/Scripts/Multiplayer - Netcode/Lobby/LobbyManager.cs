using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    private Dictionary<int, LobbyData> activeLobbies = new Dictionary<int, LobbyData>();

    private int _currentRoomIndex;

    private void Awake()
    {

    }

    public void OnRoomCreated()
    {
        activeLobbies.Add(_currentRoomIndex, new LobbyData());

        // if (!activeLobbies.ContainsKey(lobbyId))
        // {
        //     activeLobbies[lobbyId] = new LobbyData();

        //     // StartHost();
        // }
    }

    // public void JoinLobby(string lobbyId)
    // {
    //     if (activeLobbies.ContainsKey(lobbyId))
    //     {
    //         // StartClient();
    //     }
    // }
}

[System.Serializable]
public class LobbyData
{
    public string HostPlayer;
    public List<string> ConnectedPlayers = new List<string>();
}
