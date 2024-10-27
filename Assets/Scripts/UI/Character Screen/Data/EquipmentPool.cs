using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/RPG/Equipment/EquipmentPool")]
public class EquipmentPool : ScriptableObject
{
    [SerializeField] private EquipmentData[] equipmentsData;

    public EquipmentData[] EquipmentsData => equipmentsData;
}
