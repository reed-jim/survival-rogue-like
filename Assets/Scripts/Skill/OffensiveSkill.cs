using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "skill", menuName = "ScriptableObjects/RPG/OffensiveSkill")]
public class OffensiveSkill : ScriptableObject, IModifierSkill
{
    [SerializeField] private float damageMultiplier;
    [SerializeField] private float criticalChance;
    [SerializeField] private float criticalDamageMultiplier;
    [SerializeField] private float percentDirectDamage;
    [SerializeField] private float percentHealthExecuted;

    public CharacterStat GetBonusStat()
    {
        return new CharacterStat()
        {
            DamageMultiplier = damageMultiplier,
            CriticalChance = criticalChance,
            CriticalMultiplier = criticalDamageMultiplier,
            PercentDirectDamage = percentDirectDamage,
            PercentHealthExecuted = percentHealthExecuted
        };
    }

    public string GetDescription()
    {
        StringBuilder description = new StringBuilder();

        if (damageMultiplier > 0)
        {
            description.Append($"Increases your damage modifier by an additional {damageMultiplier * 100}%. ");
        }

        if (criticalChance > 0)
        {
            description.Append($"Increases your critical damage chance by an additional {criticalChance * 100}%. ");
        }

        if (criticalDamageMultiplier > 0)
        {
            description.Append($"Increases your critical damage modifier by an additional {criticalDamageMultiplier * 100}%.");
        }

        if (percentDirectDamage > 0)
        {
            description.Append($"{percentDirectDamage * 100}% of your damage will be direct damage.");
        }

        if (percentHealthExecuted > 0)
        {
            description.Append($"if enemies health are under {percentHealthExecuted * 100}% then executed them immediately");
        }

        return description.ToString();
    }

    public string GetName()
    {
        return name;
    }

    public int GetTier()
    {
        return 1;
    }
}