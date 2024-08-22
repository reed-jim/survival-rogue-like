using System.Collections;
using UnityEngine;

public static class DamageCalculator
{
    public static int GetDamage
    (
        CharacterStat attacker,
        CharacterStat target
    )
    {
        float finalDamage = 0;

        // // check if is hit
        // bool isHit = false;

        // float finalBlockChance = baseBlockChance - reducedBlockChance;

        // isHit = Random.Range(0, 100) / 100 > finalBlockChance;

        // if (!isHit)
        // {
        //     return 0;
        // }

        // damage output
        // float finalCriticalChance = attacker.CriticalChance - reducedCriticalChance;
        float finalCriticalChance = attacker.CriticalChance;

        bool isCritical = Random.Range(0, 100) / 100f < finalCriticalChance;

        if (isCritical)
        {
            finalDamage += attacker.Damage * attacker.CriticalMultiplier;
        }
        else
        {
            finalDamage += attacker.Damage;
        }

        finalDamage *= attacker.DamageMultiplier;
        // finalDamage *= (1 + increasedDamagePercent);

        // damage reduced
        // finalDamage *= damageDivider;

        float percentDamageReducedByArmor = 1 - 1 / (1 + (target.Armor * target.Armor / 1000f));

        finalDamage *= (1 - percentDamageReducedByArmor);
        // finalDamage *= (1 - wardSave);

        return (int)finalDamage;
    }
}
