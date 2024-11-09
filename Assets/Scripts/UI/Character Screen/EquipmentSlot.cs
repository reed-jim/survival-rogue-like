using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, ISaferioPageViewSlot
{
    [SerializeField] private RectTransform container;
    [SerializeField] private RectTransform iconRT;

    [SerializeField] private Image icon;
    [SerializeField] private Button selectButton;

    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private EquipmentPool equipmentPool;
    [SerializeField] private EquipmentSkillObserver equipmentSkillObserver;
    [SerializeField] private EquipmentVisualProvider equipmentVisualProvider;

    #region PRIVATE FIELD
    private OwnedEquipmentData _equipmentSlotData;
    #endregion

    #region ACTION
    public static event Action<OwnedEquipmentData> openEquipmentDetailEvent;
    #endregion

    public void Setup(int slotIndex)
    {
        RegisterButton();

        ResetState(slotIndex);
    }

    public void ResetState(int slotIndex)
    {
        if (equipmentSkillObserver.OwnedItemDatum == null)
        {
            equipmentSkillObserver.OwnedItemDatum = new List<OwnedEquipmentData>();
        }
        
        if (slotIndex < equipmentSkillObserver.OwnedItemDatum.Count)
        {
            gameObject.SetActive(true);

            int iconIndex = equipmentSkillObserver.OwnedItemDatum[slotIndex].IconIndex;

            Sprite iconSprite = equipmentVisualProvider.EquipmentSprites[iconIndex];

            icon.sprite = iconSprite;

            UIUtil.SetSizeKeepRatioX(iconRT, 0.5f * container.sizeDelta.y);

            _equipmentSlotData = equipmentSkillObserver.OwnedItemDatum[slotIndex];
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void RegisterButton()
    {
        selectButton.onClick.AddListener(Select);
    }

    private void GenerateUI()
    {

    }

    private void Select()
    {
        openEquipmentDetailEvent?.Invoke(_equipmentSlotData);
    }
}
