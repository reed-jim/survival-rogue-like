using System.Collections;
using System.Collections.Generic;
using Saferio.Util.SaferioTween;
using Unity.Netcode;
using UnityEngine;

public class NetworkedObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private JoystickController joystickController;
    [SerializeField] private RectTransform joystickControllerContainer;
    [SerializeField] private GameObject enemySpawner;

    private Vector3 _spawnPosition;
    private bool _isEnemySpawnerSpawned;

    private void Awake()
    {
        SaferioTween.DelayAsync(5, onCompletedAction: () => SpawnPlayers());

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
        if (NetworkManager.Singleton.IsServer)
        {
            foreach (var clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                GameObject player = Instantiate(playerPrefab);

                player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);

                player.transform.position = _spawnPosition;

                _spawnPosition += new Vector3(4, 0, 0);

                SpawnJoystickController(clientId);
            }

            Instantiate(enemySpawner).GetComponent<NetworkObject>().Spawn();
        }
    }

    private void SpawnJoystickController(ulong clientId)
    {
        GameObject spawnedJoystickController = Instantiate(joystickController.gameObject, joystickControllerContainer);

        NetworkObject networkJoystickController = spawnedJoystickController.GetComponent<NetworkObject>();

        networkJoystickController.SpawnAsPlayerObject(clientId);

        networkJoystickController.TrySetParent(joystickControllerContainer);
    }
}
