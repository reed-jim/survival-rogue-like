using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    [SerializeField] private PlayerStat playerStat;

    private List<EnemyStat> _enemyStats;

    public static event Action<float, float> updateExpProgressBarEvent;

    public static event Action<int, float> updateEnemyUIEvent;

    private void Awake()
    {
        Enemy.enemySpawnedEvent += OnEnemySpawned;
        Enemy.enemyHitEvent += OnEnemyHit;
        Enemy.enemyDieEvent += EarnPlayerExpKillingEnemy;

        playerStat = DataUtility.Load(new PlayerStat());

        _enemyStats = new List<EnemyStat>();
    }

    private void OnDestroy()
    {
        Enemy.enemySpawnedEvent -= OnEnemySpawned;
        Enemy.enemyHitEvent -= OnEnemyHit;
        Enemy.enemyDieEvent -= EarnPlayerExpKillingEnemy;
    }

    private void OnEnemySpawned(EnemyStat enemyStat)
    {
        _enemyStats.Add(enemyStat);
    }

    private void OnEnemyHit(int enemyIndex)
    {
        _enemyStats[enemyIndex].MinusHP(playerStat.Damage);

        updateEnemyUIEvent?.Invoke(enemyIndex, _enemyStats[enemyIndex].HP);
    }

    private void EarnPlayerExpKillingEnemy(int enemyLevel)
    {
        playerStat.EarnExp(playerStat.GetExpFromKillEnemy(enemyLevel));

        updateExpProgressBarEvent?.Invoke(playerStat.EXP, playerStat.GetRequiredExpForNextLevel());
    }
}
