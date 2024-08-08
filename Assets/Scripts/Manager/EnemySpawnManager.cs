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

    [Header("MANAGEMENT")]
    private int _currentEnemyIndex;
    private bool _isFinishAssigningEnemyIndexes;

    public static event Action<int> setEnemyIndexEvent;

    private void Awake()
    {
        enemies = new GameObject[10];

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
        while (true)
        {
            enemies[_currentEnemyIndex].transform.position = GetRandomPosition();
            enemies[_currentEnemyIndex].SetActive(true);

            if (_isFinishAssigningEnemyIndexes == false)
            {
                setEnemyIndexEvent?.Invoke(_currentEnemyIndex);
            }

            _currentEnemyIndex++;

            if (_currentEnemyIndex >= enemies.Length)
            {
                _currentEnemyIndex = 0;

                _isFinishAssigningEnemyIndexes = true;
            }

            yield return new WaitForSeconds(5);
        }
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 position = new Vector3();

        int[] allDirections = new int[] { 1, -1 };

        position.x = player.transform.position.x + allDirections[UnityEngine.Random.Range(0, 2)] * UnityEngine.Random.Range(10, 15);
        position.y = 1.3f;
        position.z = player.transform.position.y + allDirections[UnityEngine.Random.Range(0, 2)] * UnityEngine.Random.Range(10, 15);

        return position;
    }
}
