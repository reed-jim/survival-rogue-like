using System.Collections;
using System.Collections.Generic;
using ReedJim.RPG.Stat;
using UnityEngine;

public interface IDamageCalculator
{
    public int GetDamage
    (
        CharacterStat attacker,
        CharacterStat target
    );

    public bool IsCritical(int finalDamage, CharacterStat attackerStat);
}
