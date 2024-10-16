using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, ISaferioPageViewSlot
{
    [SerializeField] private Image icon;

    [SerializeField] private EquipmentSlotDataContainer equipmentSlotDataContainer;

    public void Setup(int slotIndex)
    {
        icon.sprite = equipmentSlotDataContainer.Items[slotIndex].Icon;
    }
}
