using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/RPG/EquipmentSlotData")]
public class EquipmentSlotData : ScriptableObject
{
    [SerializeField] private Sprite icon;

    public Sprite Icon => icon;
}
