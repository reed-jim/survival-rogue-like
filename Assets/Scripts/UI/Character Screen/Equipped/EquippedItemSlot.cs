using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedItemSlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Button selectButton;

    [SerializeField] private EquipmentSlotDataContainer equipmentSlotDataContainer;

    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private EquipmentVisualProvider equipmentVisualProvider;

    #region PRIVATE FIELD
    private RectTransform _container;
    private EquipmentSlotData _equipmentSlotData;
    #endregion

    #region ACTION
    public static event Action<EquipmentSlotData> openEquipmentDetailEvent;
    #endregion

    private void Awake()
    {
        icon.gameObject.SetActive(false);

        RegisterButton();
    }

    public void Setup(OwnedEquipmentData equipmentData)
    {
        icon.gameObject.SetActive(true);
        icon.sprite = equipmentVisualProvider.EquipmentSprites[equipmentData.IconIndex];
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
