using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/RPG/EquipmentSlotDataContainer")]
public class EquipmentSlotDataContainer : ScriptableObject
{
    [SerializeField] private EquipmentSlotData[] items;

    public EquipmentSlotData[] Items => items;
}
