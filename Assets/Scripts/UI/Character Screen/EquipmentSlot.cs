using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, ISaferioPageViewSlot
{
    [SerializeField] private RectTransform container;
    [SerializeField] private RectTransform iconRT;

    [SerializeField] private Image containerImage;
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
    public static event Action<OwnedEquipmentData, bool> openEquipmentDetailEvent;
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

            containerImage.color = ColorUtil.GetColorFromHex(SurvivoriumTheme.RARITY_COLORs[equipmentSkillObserver.OwnedItemDatum[slotIndex].Rarity]);
            icon.sprite = GetIconSprite(slotIndex);

            UIUtil.SetSizeKeepRatioX(iconRT, 0.5f * container.sizeDelta.y);

            _equipmentSlotData = equipmentSkillObserver.OwnedItemDatum[slotIndex];
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private Sprite GetIconSprite(int slotIndex)
    {
        int iconIndex = equipmentSkillObserver.OwnedItemDatum[slotIndex].IconIndex;

        return equipmentVisualProvider.EquipmentSprites[iconIndex];
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
        openEquipmentDetailEvent?.Invoke(_equipmentSlotData, true);
    }
}
