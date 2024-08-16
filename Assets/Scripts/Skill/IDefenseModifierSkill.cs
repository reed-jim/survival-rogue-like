using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDefenseModifierSkill
{
    public int HP
    {
        get; set;
    }

    public int Armor
    {
        get; set;
    }

    public int BlockChance
    {
        get; set;
    }

    public CharacterStat GetBonusStat();
}
