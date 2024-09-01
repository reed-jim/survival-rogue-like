using System;
using ReedJim.RPG.Stat;
using UnityEngine;

[System.Serializable]
public class PlayerStat : CharacterStat
{
    public static event Action<float, float> updateExpProgressBarEvent;

    [SerializeField] private float exp;

    public float EXP
    {
        get => exp;
        set => exp = value;
    }

    public PlayerStat(CharacterStat characterStat)
    {
        statComponents = characterStat.StatComponents;
        
        // Level = 1;
        // HP = 100;
        // MaxHP = 100;
        // Damage = 20;
        // DamageMultiplier = 1;
        // CriticalChance = 0.5f;
        // CriticalMultiplier = 2;
    }

    // public static PlayerStat Load()
    // {
    //     return DataUtility.Load(Constants.STAT_DATA_FILE_NAME, Constants.PLAYER_TAG, new PlayerStat());
    // }

    public void EarnExp(float earnedExp, out bool isLeveledUp)
    {
        isLeveledUp = false;

        exp += earnedExp;

        if (exp > GetRequiredExpForNextLevel())
        {
            ModifyStat(StatComponentNameConstant.Level, new FlatStatModifier(), 1);

            exp = 0;

            isLeveledUp = true;
        }

        DataUtility.Save(Constants.STAT_DATA_FILE_NAME, Constants.PLAYER_TAG, this);
    }

    public float GetMaxHp()
    {
        return 100;
    }

    public float GetExpFromKillEnemy(int enemyLevel)
    {
        return 45 * (GetStatValue(StatComponentNameConstant.Level) + enemyLevel - 1);
    }

    public float GetRequiredExpForNextLevel()
    {
        int level = (int)GetStatValue(StatComponentNameConstant.Level);

        return 100 * level + 5 * level * (level - 1);
    }

    public static PlayerStat operator +(PlayerStat currentStat, CharacterStat bonusStat)
    {
        PlayerStat newStat = currentStat;

        FlatStatModifier flatStatModifier = new FlatStatModifier();

        foreach (var key in newStat.GetStatKeys())
        {
            newStat.ModifyStat(key, flatStatModifier, bonusStat.GetStatBaseValue(key));
        }

        // modifiedStat.HP += bonusStat.HP;
        // modifiedStat.Armor += bonusStat.Armor;
        // modifiedStat.BlockChance += bonusStat.BlockChance;
        // modifiedStat.Damage += bonusStat.Damage;
        // modifiedStat.DamageMultiplier += bonusStat.DamageMultiplier;
        // modifiedStat.CriticalChance += bonusStat.CriticalChance;
        // modifiedStat.CriticalMultiplier += bonusStat.CriticalMultiplier;

        // modifiedStat.MovementSpeed += bonusStat.MovementSpeed;

        // modifiedStat.TakenDamageMultiplier += bonusStat.TakenDamageMultiplier;
        // modifiedStat.TakenDamageCriticalChance += bonusStat.TakenDamageCriticalChance;
        // modifiedStat.TakenDamageCriticalMultiplier += bonusStat.TakenDamageCriticalMultiplier;

        // modifiedStat.PercentDirectDamage += bonusStat.PercentDirectDamage;
        // modifiedStat.PercentHealthExecuted += bonusStat.PercentHealthExecuted;

        return newStat;
    }
}
