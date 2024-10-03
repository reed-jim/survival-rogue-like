using System;
using System.Collections.Generic;
using ReedJim.RPG.Stat;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    #region PRIVATE FIELD
    private Dictionary<int, CharacterStat> _characterStats;
    private PlayerStat _playerStat;
    #endregion

    #region ACTION
    public static event Action<float, float> updateExpProgressBarEvent;
    public static event Action showUpgradePanelEvent;
    public static event Action<float, float> setPlayerHpEvent;
    #endregion

    #region LIFE CYCLE
    private void Awake()
    {
        _characterStats = new Dictionary<int, CharacterStat>();

        CharacterStatManager.addCharacterStatToListEvent += AddCharacterStat;
        PlayerStatManager.addPlayerStatToListEvent += AddPlayerStat;
        MeleeWeapon.getAttackerStatAction += GetStat;
        Bullet.getAttackerStatAction += GetStat;
        CollectibleExperienceShard.earnPlayerExperienceEvent += EarnPlayerExpKillingEnemy;
        OffensiveSkill.updatePlayerStat += UpdatePlayerStat;
        DefenseSkill.updatePlayerStat += UpdatePlayerStat;
    }

    private void OnDestroy()
    {
        CharacterStatManager.addCharacterStatToListEvent -= AddCharacterStat;
        PlayerStatManager.addPlayerStatToListEvent -= AddPlayerStat;
        MeleeWeapon.getAttackerStatAction -= GetStat;
        Bullet.getAttackerStatAction -= GetStat;
        CollectibleExperienceShard.earnPlayerExperienceEvent -= EarnPlayerExpKillingEnemy;
        OffensiveSkill.updatePlayerStat -= UpdatePlayerStat;
        DefenseSkill.updatePlayerStat -= UpdatePlayerStat;
    }
    #endregion

    private void AddCharacterStat(int instanceId, CharacterStat stat)
    {
        if (!_characterStats.ContainsKey(instanceId))
        {
            _characterStats.Add(instanceId, stat);
        }
    }

    private void AddPlayerStat(int instanceId, PlayerStat stat)
    {
        _playerStat = stat;

        AddCharacterStat(instanceId, stat);
    }

    private CharacterStat GetStat(int instanceId)
    {
        return _characterStats[instanceId];
    }

    private void EarnPlayerExpKillingEnemy(int enemyLevel)
    {
        bool isLeveledUp;

        _playerStat.EarnExp(_playerStat.GetExpFromKillEnemy(enemyLevel), out isLeveledUp);

        updateExpProgressBarEvent?.Invoke(_playerStat.GetStatValue(StatComponentNameConstant.Experience), _playerStat.GetRequiredExpForNextLevel());

        if (isLeveledUp)
        {
            showUpgradePanelEvent?.Invoke();
        }
    }

    private void UpdatePlayerStat(CharacterStat modifierStat)
    {
        _playerStat += modifierStat;

        IStatComponent health = _playerStat.GetStat(StatComponentNameConstant.Health);

        setPlayerHpEvent?.Invoke(health.Value, health.BaseValue);
    }

    // [SerializeField] private PlayerStat playerStat;

    // [Header("SCRIPTABLE OBJECT")]
    // [SerializeField] private PlayerStatObserver playerStatObserver;

    // [Header("MANAGEMENT")]
    // private List<EnemyStat> _enemyStats;
    // private List<int> _enemySpawnTimes;

    // public static event Action<float, float> updatePlayerHpBarEvent;
    // public static event Action<float, float> updateExpProgressBarEvent;

    // public static event Action<int, float> updateEnemyUIEvent;
    // public static event Action showUpgradePanelEvent;

    // private void Awake()
    // {
    //     Enemy.enemySpawnedEvent += OnEnemySpawned;
    //     Enemy.enemyHitEvent += OnEnemyHit;
    //     Enemy.enemyDieEvent += EarnPlayerExpKillingEnemy;
    //     Enemy.playerGotHitEvent += OnPlayerHit;
    //     Enemy.resetEnemyEvent += ResetEnemy;
    //     PlayerController.getStatEvent += GetPlayerStat;
    //     PlayerController.playerGotHitEvent += OnPlayerHit;
    //     LevelingUI.upgradePlayerStatEvent += UpgradePlayerStat;

    //     PlayerStat starterPlayerStat = new PlayerStat()
    //     {
    //         Level = 1,
    //         HP = 100,
    //         Damage = 15,
    //         AttackRange = 12,
    //         ReloadTime = 1
    //     };

    //     playerStat = DataUtility.Load(defaultValue: starterPlayerStat);

    //     playerStatObserver.PlayerStat = playerStat;

    //     _enemyStats = new List<EnemyStat>();
    //     _enemySpawnTimes = new List<int>();
    // }

    // private void TestDamageOverTime()
    // {

    // }

    // private void OnDestroy()
    // {
    //     Enemy.enemySpawnedEvent -= OnEnemySpawned;
    //     Enemy.enemyHitEvent -= OnEnemyHit;
    //     Enemy.enemyDieEvent -= EarnPlayerExpKillingEnemy;
    //     Enemy.playerGotHitEvent -= OnPlayerHit;
    //     Enemy.resetEnemyEvent -= ResetEnemy;
    //     PlayerController.getStatEvent -= GetPlayerStat;
    //     PlayerController.playerGotHitEvent -= OnPlayerHit;
    //     LevelingUI.upgradePlayerStatEvent -= UpgradePlayerStat;
    // }

    // private void OnEnemySpawned(EnemyStat enemyStat)
    // {
    //     _enemyStats.Add(enemyStat);
    //     _enemySpawnTimes.Add(0);
    // }

    // private void OnEnemyHit(int enemyIndex)
    // {
    //     _enemyStats[enemyIndex].MinusHP(playerStat.Damage);

    //     updateEnemyUIEvent?.Invoke(enemyIndex, playerStat.Damage);
    // }

    // private void ResetEnemy(int enemyIndex)
    // {
    //     if (enemyIndex < _enemyStats.Count)
    //     {
    //         _enemySpawnTimes[enemyIndex]++;

    //         _enemyStats[enemyIndex].GetStronger(_enemySpawnTimes[enemyIndex]);
    //     }
    // }

    // private void OnPlayerHit(float damage)
    // {
    //     playerStat.MinusHP(damage);

    //     updatePlayerHpBarEvent?.Invoke(playerStat.HP, playerStat.GetMaxHp());
    // }

    // private void UpgradePlayerStat(StatType statType, float value)
    // {
    //     if (statType == StatType.HP)
    //     {
    //         playerStat.HP += value;
    //     }
    //     else if (statType == StatType.DAMAGE)
    //     {
    //         playerStat.Damage += value;
    //     }
    //     else if (statType == StatType.ATTACK_SPEED)
    //     {
    //         playerStat.AttackSpeed += value;
    //     }
    // }

    // private PlayerStat GetPlayerStat()
    // {
    //     return playerStat;
    // }
}
