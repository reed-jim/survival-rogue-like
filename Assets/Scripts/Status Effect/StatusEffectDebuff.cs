using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectDebuff : IModifierSkill
{
    [SerializeField] private float damageMultiplier;
    [SerializeField] private float takenDamageMultiplier;
    [SerializeField] private float takenDamageCriticalChance;
    [SerializeField] private float takenDamageCriticalMultiplier;

    public CharacterStat GetBonusStat()
    {
        return new CharacterStat()
        {
            DamageMultiplier = damageMultiplier,
            TakenDamageMultiplier = takenDamageMultiplier,
            TakenDamageCriticalChance = takenDamageCriticalChance,
            TakenDamageCriticalMultiplier = takenDamageCriticalMultiplier
        };
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
