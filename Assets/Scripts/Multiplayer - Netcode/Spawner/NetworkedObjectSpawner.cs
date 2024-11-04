using System.Collections;
using System.Collections.Generic;
using Saferio.Util.SaferioTween;
using Unity.Netcode;
using UnityEngine;

public class NetworkedObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemySpawner;

    private void Awake()
    {
        SaferioTween.DelayAsync(10, onCompletedAction: () => Spawn());
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
}
