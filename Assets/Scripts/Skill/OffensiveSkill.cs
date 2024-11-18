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
        StringBuilder description = new StringBuilder();

        string colorStringByTier = SurvivoriumTheme.RARITY_COLORs[_currentTier];

        if (damageMultiplier > 0)
        {
            description.Append($"Increases your damage modifier by an additional <color={colorStringByTier}>{damageMultiplier * 100 * (_currentTier + 1)}%</color>. ");
        }

        if (criticalChance > 0)
        {
            description.Append($"Increases your critical damage chance by an additional <color={colorStringByTier}>{criticalChance * 100 * (_currentTier + 1)}%</color>. ");
        }

        if (criticalDamageMultiplier > 0)
        {
            description.Append($"Increases your critical damage modifier by an additional <color={colorStringByTier}>{criticalDamageMultiplier * 100 * (_currentTier + 1)}%</color>.");
        }

        if (percentDirectDamage > 0)
        {
            description.Append($"<color={colorStringByTier}>{percentDirectDamage * 100 * (_currentTier + 1)}%</color> of your damage will be direct damage.");
        }

        if (percentHealthExecuted > 0)
        {
            description.Append($"if enemies health are under <color={colorStringByTier}>{percentHealthExecuted * 100 * (_currentTier + 1)}%</color> then executed them immediately");
        }

        return description.ToString();
    }

    public override string GetName()
    {
        return name;
    }

    public override int GetTier()
    {
        if (_currentTier == -1)
        {
            _currentTier = UnityEngine.Random.Range(0, 5);
        }

        return _currentTier;
    }

    public override void AddSkill()
    {
        updatePlayerStat?.Invoke(GetBonusStat());
    }
}
