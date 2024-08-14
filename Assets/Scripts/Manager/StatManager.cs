using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    [SerializeField] private PlayerStat playerStat;

    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private PlayerStatObserver playerStatObserver;

    [Header("MANAGEMENT")]
    private List<EnemyStat> _enemyStats;
    private List<int> _enemySpawnTimes;

    public static event Action<float, float> updatePlayerHpBarEvent;
    public static event Action<float, float> updateExpProgressBarEvent;

    public static event Action<int, float> updateEnemyUIEvent;
    public static event Action showUpgradePanelEvent;

    private void Awake()
    {
        Enemy.enemySpawnedEvent += OnEnemySpawned;
        Enemy.enemyHitEvent += OnEnemyHit;
        Enemy.enemyDieEvent += EarnPlayerExpKillingEnemy;
        Enemy.playerGotHitEvent += OnPlayerHit;
        Enemy.resetEnemyEvent += ResetEnemy;
        PlayerController.getStatEvent += GetPlayerStat;
        PlayerController.playerGotHitEvent += OnPlayerHit;
        LevelingUI.upgradePlayerStatEvent += UpgradePlayerStat;

        PlayerStat starterPlayerStat = new PlayerStat()
        {
            HP = 100,
            Damage = 15,
            AttackRange = 12,
            ReloadTime = 1
        };

        playerStat = DataUtility.Load(defaultValue: starterPlayerStat);

        playerStatObserver.PlayerStat = playerStat;

        _enemyStats = new List<EnemyStat>();
        _enemySpawnTimes = new List<int>();
    }

    private void OnDestroy()
    {
        Enemy.enemySpawnedEvent -= OnEnemySpawned;
        Enemy.enemyHitEvent -= OnEnemyHit;
        Enemy.enemyDieEvent -= EarnPlayerExpKillingEnemy;
        Enemy.playerGotHitEvent -= OnPlayerHit;
        Enemy.resetEnemyEvent -= ResetEnemy;
        PlayerController.getStatEvent -= GetPlayerStat;
        PlayerController.playerGotHitEvent -= OnPlayerHit;
        LevelingUI.upgradePlayerStatEvent -= UpgradePlayerStat;
    }

    private void OnEnemySpawned(EnemyStat enemyStat)
    {
        _enemyStats.Add(enemyStat);
        _enemySpawnTimes.Add(0);
    }

    private void OnEnemyHit(int enemyIndex)
    {
        _enemyStats[enemyIndex].MinusHP(playerStat.Damage);

        updateEnemyUIEvent?.Invoke(enemyIndex, playerStat.Damage);
    }

    private void ResetEnemy(int enemyIndex)
    {
        if (enemyIndex < _enemyStats.Count)
        {
            _enemySpawnTimes[enemyIndex]++;

            _enemyStats[enemyIndex].GetStronger(_enemySpawnTimes[enemyIndex]);
        }
    }

    private void OnPlayerHit(float damage)
    {
        playerStat.MinusHP(damage);

        updatePlayerHpBarEvent?.Invoke(playerStat.HP, playerStat.GetMaxHp());
    }

    private void EarnPlayerExpKillingEnemy(int enemyLevel)
    {
        bool isLeveledUp;

        playerStat.EarnExp(playerStat.GetExpFromKillEnemy(enemyLevel), out isLeveledUp);

        updateExpProgressBarEvent?.Invoke(playerStat.EXP, playerStat.GetRequiredExpForNextLevel());

        if (isLeveledUp)
        {
            showUpgradePanelEvent?.Invoke();
        }
    }

    private void UpgradePlayerStat(StatType statType, float value)
    {
        if (statType == StatType.HP)
        {
            playerStat.HP += value;
        }
        else if (statType == StatType.DAMAGE)
        {
            playerStat.Damage += value;
        }
        else if (statType == StatType.ATTACK_SPEED)
        {
            playerStat.AttackSpeed += value;
        }
    }

    private PlayerStat GetPlayerStat()
    {
        return playerStat;
    }
}
