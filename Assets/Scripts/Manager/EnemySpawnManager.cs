using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private GameObject[] enemies;

    [Header("PLAYER")]
    [SerializeField] private Transform player;

    [Header("MANAGEMENT")]
    private int _currentEnemyIndex;

    private void Awake()
    {
        enemies = new GameObject[10];

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i] = Instantiate(enemyPrefab);
            enemies[i].SetActive(false);
        }

        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            enemies[_currentEnemyIndex].transform.position = GetRandomPosition();
            enemies[_currentEnemyIndex].SetActive(true);

            _currentEnemyIndex++;

            if (_currentEnemyIndex >= enemies.Length)
            {
                _currentEnemyIndex = 0;
            }

            yield return new WaitForSeconds(10);
        }
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 position = new Vector3();

        int[] allDirections = new int[] { 1, -1 };

        position.x = player.transform.position.x + allDirections[Random.Range(0, 1)] * Random.Range(10, 15);
        position.y = 0.6f;
        position.z = player.transform.position.y + allDirections[Random.Range(0, 1)] * Random.Range(10, 15);

        return position;
    }
}
