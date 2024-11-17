using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquippedItemSlot : MonoBehaviour
{
    [SerializeField] private RectTransform container;
    [SerializeField] private RectTransform iconRT;
    [SerializeField] private RectTransform levelTextBackground;
    [SerializeField] private RectTransform levelTextRT;

    [SerializeField] private Image containerBackground;
    [SerializeField] private Image icon;
    [SerializeField] private Button selectButton;
    [SerializeField] private TMP_Text levelText;

    [SerializeField] private EquipmentSlotDataContainer equipmentSlotDataContainer;

    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private EquipmentVisualProvider equipmentVisualProvider;

    #region PRIVATE FIELD
    private OwnedEquipmentData _ownedEquipmentData;
    #endregion

    #region ACTION
    public static event Action<OwnedEquipmentData, bool> openEquipmentDetailEvent;
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

        UIUtil.SetSizeKeepRatioX(iconRT, 0.6f * container.sizeDelta.y);

        containerBackground.color = ColorUtil.GetColorFromHex(SurvivoriumTheme.RARITY_COLORs[equipmentData.Rarity]);

        levelText.text = $"Lv {equipmentData.Level}";

        levelTextBackground.gameObject.SetActive(true);

        _ownedEquipmentData = equipmentData;
    }

    public void Unequip()
    {
        icon.gameObject.SetActive(false);

        containerBackground.color = ColorUtil.GetColorFromHex(SurvivoriumTheme.DEFAULT_EQUIPMENT_SLOT_COLOR);

        levelTextRT.gameObject.SetActive(false);
    }

    private void RegisterButton()
    {
        selectButton.onClick.AddListener(Select);
    }

    public void GenerateUI()
    {
        float padding = 0.15f * container.sizeDelta.y;

        // UIUtil.SetLocalPosition(levelTextRT, 0.5f * container.sizeDelta - padding * Vector2.one);
        UIUtil.SetSize(levelTextBackground, 0.7f * container.sizeDelta.x, 0.25f * container.sizeDelta.x);
        UIUtil.SetLocalPositionY(levelTextBackground, 0.5f * container.sizeDelta.y);

        UIUtil.SetFontSizeOnly(levelText, 0.2f * container.sizeDelta.y);

        levelTextBackground.gameObject.SetActive(false);
    }

    private void Select()
    {
        openEquipmentDetailEvent?.Invoke(_ownedEquipmentData, false);
    }
}
