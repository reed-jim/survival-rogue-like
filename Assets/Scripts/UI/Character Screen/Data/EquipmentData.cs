using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/RPG/Equipment/EquipmentData")]
[Serializable]
public class EquipmentData : ScriptableObject
{
    [SerializeField] private string equipmentName;
    [SerializeField] private int iconIndex;
    [SerializeField] private BaseSkill skill;

    public string EquipmentName => name;
    public int IconIndex => iconIndex;
    public BaseSkill Skill => skill;
}

