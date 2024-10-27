using System;
using System.Collections;
using System.Collections.Generic;
using ReedJim.RPG.Stat;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/RPG/EquipmentSlotData")]
[Serializable]
public class EquipmentSlotData : ScriptableObject
{
    [SerializeField] private Sprite icon;
    [SerializeField] private BaseSkill skill;

    public Sprite Icon => icon;
    public BaseSkill Skill => skill;
}
