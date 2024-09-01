using System.Collections;
using System.Collections.Generic;
using ReedJim.RPG.Stat;
using UnityEngine;

public class StatusEffectDebuff : IModifierSkill
{
    [SerializeField] private float damageMultiplier;
    [SerializeField] private float takenDamageMultiplier;
    [SerializeField] private float takenDamageCriticalChance;
    [SerializeField] private float takenDamageCriticalMultiplier;

    public CharacterStat GetBonusStat()
    {
        CharacterStat bonusStat = new CharacterStat();

        bonusStat.SetStatBaseValue(StatComponentNameConstant.DamageMultiplier, damageMultiplier);
        bonusStat.SetStatBaseValue(StatComponentNameConstant.TakenDamageMultiplier, takenDamageMultiplier);
        bonusStat.SetStatBaseValue(StatComponentNameConstant.TakenDamageCriticalChance, takenDamageCriticalChance);
        bonusStat.SetStatBaseValue(StatComponentNameConstant.TakenDamageCriticalMultiplier, takenDamageCriticalMultiplier);

        return bonusStat;
    }

    public string GetDescription()
    {
        throw new System.NotImplementedException();
    }

    public string GetName()
    {
        throw new System.NotImplementedException();
    }

    public int GetTier()
    {
        throw new System.NotImplementedException();
    }
}
