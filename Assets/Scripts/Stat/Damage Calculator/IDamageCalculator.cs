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
}
