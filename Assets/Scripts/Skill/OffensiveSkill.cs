using System;
using System.Text;
using ReedJim.RPG.Stat;
using UnityEngine;

[CreateAssetMenu(fileName = "skill", menuName = "ScriptableObjects/RPG/OffensiveSkill")]
public class OffensiveSkill : BaseSkill, IModifierSkill
{
    [SerializeField] private float damageMultiplier;
    [SerializeField] private float criticalChance;
    [SerializeField] private float criticalDamageMultiplier;
    [SerializeField] private float percentDirectDamage;
    [SerializeField] private float percentHealthExecuted;

    #region ACTION
    public static event Action<CharacterStat> updatePlayerStat;
    #endregion

    private int _currentTier = -1;

    public CharacterStat GetBonusStat()
    {
        CharacterStat bonusStat = new CharacterStat();

        bonusStat.SetStatBaseValue(StatComponentNameConstant.DamageMultiplier, damageMultiplier * (_currentTier + 1));
        bonusStat.SetStatBaseValue(StatComponentNameConstant.CriticalChance, criticalChance * (_currentTier + 1));
        bonusStat.SetStatBaseValue(StatComponentNameConstant.CriticalMultiplier, criticalDamageMultiplier * (_currentTier + 1));
        bonusStat.SetStatBaseValue(StatComponentNameConstant.PercentDirectDamage, percentDirectDamage * (_currentTier + 1));
        bonusStat.SetStatBaseValue(StatComponentNameConstant.PercentHealthExecuted, percentHealthExecuted * (_currentTier + 1));

        return bonusStat;
    }

    public override string GetDescription()
    {
        _currentTier = GetTier();

        StringBuilder description = new StringBuilder();

        if (damageMultiplier > 0)
        {
            description.Append($"Increases your damage modifier by an additional <color=#FF8282>{damageMultiplier * 100 * (_currentTier + 1)}%</color>. ");
        }

        if (criticalChance > 0)
        {
            description.Append($"Increases your critical damage chance by an additional <color=#FF8282>{criticalChance * 100 * (_currentTier + 1)}%</color>. ");
        }

        if (criticalDamageMultiplier > 0)
        {
            description.Append($"Increases your critical damage modifier by an additional {criticalDamageMultiplier * 100 * (_currentTier + 1)}%.");
        }

        if (percentDirectDamage > 0)
        {
            description.Append($"{percentDirectDamage * 100 * (_currentTier + 1)}% of your damage will be direct damage.");
        }

        if (percentHealthExecuted > 0)
        {
            description.Append($"if enemies health are under {percentHealthExecuted * 100 * (_currentTier + 1)}% then executed them immediately");
        }

        return description.ToString();
    }

    public override string GetName()
    {
        return name;
    }

    public override int GetTier()
    {
        return UnityEngine.Random.Range(0, 5);
    }

    public override void AddSkill()
    {
        updatePlayerStat?.Invoke(GetBonusStat());
    }
}
