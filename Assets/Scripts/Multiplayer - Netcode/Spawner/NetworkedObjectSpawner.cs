using System.Collections;
using System.Collections.Generic;
using Saferio.Util.SaferioTween;
using Unity.Netcode;
using UnityEngine;

public class NetworkedObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemySpawner;

    private Vector3 _spawnPosition;
    private bool _isEnemySpawnerSpawned;

    private void Awake()
    {
        SaferioTween.DelayAsync(10, onCompletedAction: () => SpawnPlayers());

        // NetworkManager.Singleton.OnClientStarted += SpawnPlayer;
    }

    private void Spawn()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            GameObject player = Instantiate(playerPrefab);

            player.GetComponent<NetworkObject>().Spawn();

            Instantiate(enemySpawner).GetComponent<NetworkObject>().Spawn();
        }
    }

    private void SpawnPlayers()
    {
        Debug.Log("client");
        if (NetworkManager.Singleton.IsServer)
        {
            Debug.Log(NetworkManager.Singleton.ConnectedClientsIds.Count);
            foreach (var clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                GameObject player = Instantiate(playerPrefab);

                player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);

                player.transform.position = _spawnPosition;

                _spawnPosition += new Vector3(3, 0, 0);
            }

            Instantiate(enemySpawner).GetComponent<NetworkObject>().Spawn();

            // if (!_isEnemySpawnerSpawned)
            // {
            //     Instantiate(enemySpawner).GetComponent<NetworkObject>().Spawn();

            //     _isEnemySpawnerSpawned = true;
            // }
        }
    }
}
