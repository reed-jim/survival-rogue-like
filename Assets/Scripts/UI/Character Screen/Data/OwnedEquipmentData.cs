using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnedEquipmentData
{
    private string _name;
    private int iconIndex;
    private int _rarity;
    private int _level;

    // save index because JSON can't serialize interface ISkill, save as BaseSkill is not work too because when loaded from JSON, child class
    // will be casted to BaseSkill
    private int skillIndexInContainer;


    public BaseSkill GetSkill(SkillContainer skillContainer)
    {
        return skillContainer.AllSkills[skillIndexInContainer];
    }

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

    public int SkillIndexInContainer
    {
        get => skillIndexInContainer;
        set => skillIndexInContainer = value;
    }

    public int Rarity
    {
        get => _rarity;
        set => _rarity = value;
    }

    public int Level
    {
        get => _level;
        set => _level = value;
    }
}
