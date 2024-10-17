using System.Collections;
using System.Collections.Generic;
using ReedJim.RPG.Stat;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment Skill Observer", menuName = "ScriptableObjects/RPG/EquipmentSkillObserver")]
public class EquipmentSkillObserver : ScriptableObject
{
    private List<BaseSkill> _skillFromEquipments;

    public List<BaseSkill> SkillFromEquipments
    {
        get => _skillFromEquipments; set => _skillFromEquipments = value;
    }

    public void Add(BaseSkill skill)
    {
        if (_skillFromEquipments == null)
        {
            _skillFromEquipments = new List<BaseSkill>();
        }

        _skillFromEquipments.Add(skill);
    }
}
