using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SimplePageViewSlot : MonoBehaviour, ISaferioPageViewSlot
{
    [SerializeField] private TMP_Text contentText;

    public void Setup(int slotIndex)
    {
        contentText.text = $"Slot {slotIndex}";
    }
}
