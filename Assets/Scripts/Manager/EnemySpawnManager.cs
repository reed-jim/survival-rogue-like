using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private GameObject[] enemies;

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

            yield return new WaitForSeconds(10);
        }
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 position = new Vector3();

        position.x = Random.Range(5, 12);
        position.y = 0.6f;
        position.z = Random.Range(5, 12);

        return position;
    }
}
