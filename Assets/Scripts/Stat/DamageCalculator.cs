using System.Collections;
using ReedJim.RPG.Stat;
using UnityEngine;

public class DamageCalculator : IDamageCalculator
{
    public int GetDamage
    (
        CharacterStat attacker,
        CharacterStat target
    )
    {
        float attackerDamage = attacker.GetStatValue(StatComponentNameConstant.Damage);
        float attackerDamageMultiplier = attacker.GetStatValue(StatComponentNameConstant.DamageMultiplier);
        float attackerCriticalChance = attacker.GetStatValue(StatComponentNameConstant.CriticalChance);
        float attackerCriticalMultiplier = attacker.GetStatValue(StatComponentNameConstant.CriticalMultiplier);
        float attackerPercentDirectDamage = attacker.GetStatValue(StatComponentNameConstant.PercentDirectDamage);
        float attackerPercentHealthExecuted = attacker.GetStatValue(StatComponentNameConstant.PercentHealthExecuted);

        IStatComponent targetHealth = target.GetStat(StatComponentNameConstant.Health);

        float targetArmor = target.GetStatValue(StatComponentNameConstant.Armor);

        float finalDamage = 0;

        // // CHECK IF IS HIT
        // bool isHit = false;

        // float finalBlockChance = baseBlockChance - reducedBlockChance;

        // isHit = Random.Range(0, 100) / 100 > finalBlockChance;

        // if (!isHit)
        // {
        //     return 0;
        // }

        // DAMAGE OUTPUT
        // float finalCriticalChance = attacker.CriticalChance - reducedCriticalChance;
        float finalCriticalChance = attackerCriticalChance;

        float randomNumber = Random.Range(0, 100) / 100f;

        bool isCritical = randomNumber < finalCriticalChance;

        if (isCritical)
        {
            finalDamage += attackerDamage * attackerCriticalMultiplier;
        }
        else
        {
            finalDamage += attackerDamage;
        }

        finalDamage *= attackerDamageMultiplier;
        // finalDamage *= (1 + increasedDamagePercent);

        // DAMAGE REDUCED
        // finalDamage *= damageDivider;

        float percentDamageReducedByArmor = 1 - 1 / (1 + (targetArmor * targetArmor / 1000f));

        float directDamage = finalDamage * attackerPercentDirectDamage;
        float normalDamage = finalDamage - directDamage;

        finalDamage = directDamage + normalDamage * (1 - percentDamageReducedByArmor);
        // finalDamage *= (1 - wardSave);

        // EXECUTED
        if (attackerPercentHealthExecuted > 0)
        {
            if (targetHealth.Value - finalDamage < attackerPercentHealthExecuted * targetHealth.BaseValue)
            {
                finalDamage = targetHealth.BaseValue;
            }
        }

        return (int)finalDamage;
    }

    public bool IsCritical(int finalDamage, CharacterStat attackerStat)
    {
        if (finalDamage > attackerStat.GetStatValue(StatComponentNameConstant.Damage))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class DirectDamageCalculator : IDamageCalculator
{
    public int GetDamage(CharacterStat attacker, CharacterStat target)
    {
        throw new System.NotImplementedException();
    }

    public bool IsCritical(int finalDamage, CharacterStat attackerStat)
    {
        throw new System.NotImplementedException();
    }
}
