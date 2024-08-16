using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModifierSkill : ISkill
{
    public CharacterStat GetBonusStat();
}
