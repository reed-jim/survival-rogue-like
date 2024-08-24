using System;
using System.Collections;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] private Transform enemyContainer;
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private GameObject[] enemies;

    [Header("PLAYER")]
    [SerializeField] private Transform player;

    [Header("CUSTOMIZE")]
    [SerializeField] private int maxEnemy;
    [SerializeField] private int maxEnemySpawnConcurrently;

    [Header("MANAGEMENT")]
    private int _currentEnemyIndex;

    public static event Action<int> spawnEnemyEvent;

    private void Awake()
    {
        enemies = new GameObject[maxEnemy];

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i] = Instantiate(enemyPrefab, enemyContainer);
            enemies[i].name = $"Enemy {i}";
            enemies[i].SetActive(false);
        }

        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        // wait for enemy to be set up
        yield return new WaitForSeconds(1f);
        
        while (true)
        {
            for (int i = 0; i < maxEnemySpawnConcurrently; i++)
            {
                if (!enemies[_currentEnemyIndex].activeSelf)
                {
                    enemies[_currentEnemyIndex].transform.position = GetRandomPositionCircular();
                    enemies[_currentEnemyIndex].SetActive(true);

                    spawnEnemyEvent?.Invoke(enemies[_currentEnemyIndex].GetInstanceID());

                    _currentEnemyIndex++;

                    if (_currentEnemyIndex >= enemies.Length)
                    {
                        _currentEnemyIndex = 0;
                    }
                }
            }

            yield return new WaitForSeconds(5);
        }
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 position = new Vector3();

        int[] allDirections = new int[] { 1, -1 };

        position.x = player.transform.position.x + allDirections[UnityEngine.Random.Range(0, 2)] * UnityEngine.Random.Range(15, 30);
        position.y = 1.3f;
        position.z = player.transform.position.y + allDirections[UnityEngine.Random.Range(0, 2)] * UnityEngine.Random.Range(15, 30);

        return position;
    }

    private Vector3 GetRandomPositionCircular()
    {
        Vector3 position = new Vector3();

        int[] allDirections = new int[] { 1, -1 };

        float radius = 30;

        position.x = player.transform.position.x + allDirections[UnityEngine.Random.Range(0, 2)] * UnityEngine.Random.Range(15, 30);
        position.y = 1.3f;
        position.z = allDirections[UnityEngine.Random.Range(0, 2)] * Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(position.x, 2));

        return position;
    }
}
