using System;
using System.Collections;
using Saferio.Util.SaferioTween;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class EnemySpawnManager : NetworkBehaviour
{
    [SerializeField] private Transform enemyContainer;
    [SerializeField] private GameObject[] enemyPrefabs;

    [SerializeField] private GameObject[] enemies;
    [SerializeField] private NetworkObject[] networkEnemies;

    [Header("PLAYER")]
    [SerializeField] private PlayerRuntime playerRuntime;

    [Header("CUSTOMIZE")]
    [SerializeField] private int maxEnemy;
    [SerializeField] private int maxEnemySpawnConcurrently;
    [SerializeField] private float minDistanceSpawn;
    [SerializeField] private float maxDistanceSpawn;

    [Header("MANAGEMENT")]
    private int _currentEnemyIndex;

    public static event Action<int> spawnEnemyEvent;

    private void Awake()
    {
        // enemies = new GameObject[maxEnemy];

        networkEnemies = new NetworkObject[maxEnemy];

        if (NetworkManager.Singleton.IsServer)
        {
            for (int i = 0; i < networkEnemies.Length; i++)
            {
                // networkEnemies[i] = Instantiate(GetRandomEnemyPrefab(), enemyContainer);
                NetworkObject networkObject = Instantiate(GetRandomEnemyPrefab(), enemyContainer).GetComponent<NetworkObject>();

                // NetworkObject networkEnemy = networkEnemies[i].GetComponent<NetworkObject>();

                networkObject.Spawn();

                networkObject.name = $"Enemy {i}";
                networkObject.gameObject.SetActive(false);

                // networkEnemies[i] = networkObject;

                int index = i;

                ulong enemyNetworkId = networkObject.NetworkObjectId;

                StartCoroutine(TryAssignNetworkEnemyToArrayRpc(index, enemyNetworkId));

                StartCoroutine(TrySetActive(enemyNetworkId, false));
            }

            StartCoroutine(Spawn());
        }
    }

    [Rpc(SendTo.Everyone)]
    private void AssignNetworkEnemyToArrayRpc(int index, ulong networkObjectId)
    {
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(networkObjectId, out NetworkObject networkObject))
        {
            networkEnemies[index] = networkObject;
        }
    }

    [Rpc(SendTo.Everyone)]
    private void SetActiveRpc(ulong networkObjectId, bool isActive)
    {
        DebugUtil.DistinctLog(networkObjectId);

        foreach (var item in NetworkManager.Singleton.SpawnManager.SpawnedObjects)
        {
            Debug.Log(item.Key);
        }

        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(networkObjectId, out NetworkObject networkObject))
        {
            networkObject.gameObject.SetActive(isActive);
        }
    }

    private IEnumerator TryAssignNetworkEnemyToArrayRpc(int index, ulong networkObjectId)
    {
        yield return new WaitUntil(() => IsSpawned);

        AssignNetworkEnemyToArrayRpc(index, networkObjectId);
    }

    private IEnumerator TrySetActive(ulong objectId, bool isActive)
    {
        yield return new WaitUntil(() => IsSpawned);

        SetActiveRpc(objectId, isActive);
    }

    private IEnumerator Spawn()
    {
        // wait for enemy to be set up
        yield return new WaitForSeconds(1f);

        while (true)
        {
            if (playerRuntime.player == null)
            {
                yield return new WaitForSeconds(1f);
                yield return null;
            }

            // yield return new WaitUntil(() => IsSpawned);

            // SpawnEnemyRpc();

            for (int i = 0; i < maxEnemySpawnConcurrently; i++)
            {
                if (!networkEnemies[_currentEnemyIndex].gameObject.activeSelf)
                {
                    networkEnemies[_currentEnemyIndex].transform.position = GetRandomPositionCircular(networkEnemies[_currentEnemyIndex].transform.position);
                    networkEnemies[_currentEnemyIndex].gameObject.SetActive(true);

                    spawnEnemyEvent?.Invoke(networkEnemies[_currentEnemyIndex].gameObject.GetInstanceID());

                    _currentEnemyIndex++;

                    if (_currentEnemyIndex >= networkEnemies.Length)
                    {
                        _currentEnemyIndex = 0;
                    }
                }
            }

            yield return new WaitForSeconds(5);
        }
    }

    [Rpc(SendTo.Everyone)]
    private void SpawnEnemyRpc()
    {
        for (int i = 0; i < maxEnemySpawnConcurrently; i++)
        {
            if (!enemies[_currentEnemyIndex].activeSelf)
            {
                enemies[_currentEnemyIndex].transform.position = GetRandomPositionCircular(enemies[_currentEnemyIndex].transform.position);
                enemies[_currentEnemyIndex].SetActive(true);

                spawnEnemyEvent?.Invoke(enemies[_currentEnemyIndex].GetInstanceID());

                _currentEnemyIndex++;

                if (_currentEnemyIndex >= enemies.Length)
                {
                    _currentEnemyIndex = 0;
                }
            }
        }
    }

    // private Vector3 GetRandomPosition()
    // {
    //     Vector3 position = new Vector3();

    //     int[] allDirections = new int[] { 1, -1 };

    //     position.x = player.transform.position.x + allDirections[UnityEngine.Random.Range(0, 2)] * UnityEngine.Random.Range(minDistanceSpawn, maxDistanceSpawn);
    //     position.y = transform.position.y;
    //     position.z = player.transform.position.y + allDirections[UnityEngine.Random.Range(0, 2)] * UnityEngine.Random.Range(minDistanceSpawn, maxDistanceSpawn);

    //     return position;
    // }

    private Vector3 GetRandomPositionCircular(Vector3 currentPosition)
    {
        Vector3 position = new Vector3();

        int[] allDirections = new int[] { 1, -1 };

        float radius = 30;

        float randomDistanceXToPlayer = UnityEngine.Random.Range(15, 30);

        position.x = playerRuntime.player.position.x + allDirections[UnityEngine.Random.Range(0, 2)] * randomDistanceXToPlayer;
        position.y = currentPosition.y;
        position.z = allDirections[UnityEngine.Random.Range(0, 2)] * Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(randomDistanceXToPlayer, 2));

        return position;
    }

    private GameObject GetRandomEnemyPrefab()
    {
        int randomIndex = UnityEngine.Random.Range(0, enemyPrefabs.Length);

        return enemyPrefabs[randomIndex];

        // if (randomNumber < 5)
        // {
        //     return enemyPrefabs[1];
        // }
        // else
        // {
        //     return enemyPrefabs[0];
        // }
    }
}
