using System.Collections;
using System.Collections.Generic;
using ReedJim.RPG.Stat;
using UnityEngine;

public interface IModifierSkill : ISkill
{
    public CharacterStat GetBonusStat();
}
