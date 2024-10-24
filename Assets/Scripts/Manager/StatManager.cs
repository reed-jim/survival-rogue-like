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
    public static event Action<int> updatePlayerLevelTextEvent;
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
        GameTimeCounter.scaleEnemyPowerOverTimeEvent += ScaleEnemyPowerOverTime;
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
        GameTimeCounter.scaleEnemyPowerOverTimeEvent -= ScaleEnemyPowerOverTime;
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

            updatePlayerLevelTextEvent?.Invoke((int)_playerStat.GetStatValue(StatComponentNameConstant.Level));
        }
    }

    private void UpdatePlayerStat(CharacterStat modifierStat)
    {
        _playerStat += modifierStat;

        InvokeUpdatePlayerHealthUI();
    }

    private void InvokeUpdatePlayerHealthUI()
    {
        IStatComponent health = _playerStat.GetStat(StatComponentNameConstant.Health);

        setPlayerHpEvent?.Invoke(health.Value, health.BaseValue);
    }

    private void ScaleEnemyPowerOverTime(float timeElapsed)
    {
        foreach (var item in _characterStats.Values)
        {
            float baseValue = item.GetStatBaseValue(StatComponentNameConstant.Health);
            float currentValue = item.GetStatValue(StatComponentNameConstant.Health);

            if (currentValue < baseValue)
            {
                continue;
            }

            FlatStatModifier flatStatModifier = new FlatStatModifier();

            item.ModifyStat(StatComponentNameConstant.Health, flatStatModifier, 5 * Mathf.Pow(timeElapsed, 1.05f));
        }
    }
}
