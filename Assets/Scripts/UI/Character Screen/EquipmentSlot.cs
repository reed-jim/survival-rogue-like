using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, ISaferioPageViewSlot
{
    [SerializeField] private Image icon;
    [SerializeField] private Button selectButton;

    [SerializeField] private EquipmentSlotDataContainer equipmentSlotDataContainer;

    private RectTransform _container;

    private EquipmentSlotData _equipmentSlotData;

    #region ACTION
    public static event Action<EquipmentSlotData> openEquipmentDetailEvent;
    #endregion

    public void Setup(int slotIndex)
    {
        _container = GetComponent<RectTransform>();

        _equipmentSlotData = equipmentSlotDataContainer.Items[slotIndex];

        icon.sprite = _equipmentSlotData.Icon;

        UIUtil.SetSizeKeepRatioX(icon, 0.5f * _container.sizeDelta.y);

        RegisterButton();
    }

    private void RegisterButton()
    {
        selectButton.onClick.AddListener(Select);
    }

    private void Select()
    {
        openEquipmentDetailEvent?.Invoke(_equipmentSlotData);
    }
}
