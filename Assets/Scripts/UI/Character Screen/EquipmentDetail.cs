using System.Collections;
using System.Collections.Generic;
using ReedJim.RPG.Stat;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentDetail : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text equipmentName;
    [SerializeField] private TMP_Text description;

    private void Awake()
    {
        EquipmentSlot.openEquipmentDetailEvent += Show;

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EquipmentSlot.openEquipmentDetailEvent -= Show;
    }

    public void Setup(EquipmentSlotData data)
    {
        icon.sprite = data.Icon;
    }

    private void Show(EquipmentSlotData data)
    {
        gameObject.SetActive(true);

        icon.sprite = data.Icon;

        equipmentName.text = data.name;

        description.text = data.Skill.GetDescription();
    }
}
