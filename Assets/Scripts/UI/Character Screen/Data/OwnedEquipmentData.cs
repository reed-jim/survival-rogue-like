using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnedEquipmentData
{
    private string _name;
    private int iconIndex;
    private BaseSkill skill;
    private int _rarity;

    public string Name
    {
        get => _name;
        set => _name = value;
    }

    public int IconIndex
    {
        get => iconIndex;
        set => iconIndex = value;
    }

    public BaseSkill Skill
    {
        get => skill;
        set => skill = value;
    }

    public int Rarity
    {
        get => _rarity;
        set => _rarity = value;
    }
}
