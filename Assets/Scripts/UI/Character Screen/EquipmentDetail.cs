using System;
using System.Collections;
using System.Collections.Generic;
using ReedJim.RPG.Stat;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentDetail : SaferioPopup
{
    [SerializeField] private RectTransform canvas;
    [SerializeField] private RectTransform container;
    [SerializeField] private RectTransform fadeBackground;
    [SerializeField] private RectTransform iconBackgroundRT;
    [SerializeField] private RectTransform iconRT;
    [SerializeField] private RectTransform equipmentNameRT;
    [SerializeField] private RectTransform tagRarityRT;
    [SerializeField] private RectTransform descriptionBackground;
    [SerializeField] private RectTransform descriptionRT;
    [SerializeField] private RectTransform closeButtonRT;
    [SerializeField] private RectTransform equipButtonRT;

    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text equipmentName;
    [SerializeField] private Image rarirtyTag;
    [SerializeField] private TMP_Text rarirtyText;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text equipButtonText;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button equipButton;

    [Header("SCRIPTABLE OBJECTS")]
    [SerializeField] private EquipmentSkillObserver equipmentSkillObserver;
    [SerializeField] private EquipmentVisualProvider equipmentVisualProvider;
    [SerializeField] private SkillContainer skillContainer;

    #region PRIVATE FIELD
    private Vector2 _canvasSize;
    private OwnedEquipmentData _data;
    #endregion

    #region ACTION
    public static event Action refreshCharacterScreenEvent;
    #endregion

    private void Awake()
    {
        EquipmentSlot.openEquipmentDetailEvent += ShowAndRefresh;
        EquippedItemSlot.openEquipmentDetailEvent += ShowAndRefresh;

        _canvasSize = canvas.sizeDelta;

        RegisterButton();

        GenerateUI();

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EquipmentSlot.openEquipmentDetailEvent -= ShowAndRefresh;
        EquippedItemSlot.openEquipmentDetailEvent -= ShowAndRefresh;
    }

    public void Setup(OwnedEquipmentData data)
    {
        icon.sprite = equipmentVisualProvider.EquipmentSprites[data.IconIndex];
    }

    private void RegisterButton()
    {
        closeButton.onClick.AddListener(Close);
    }

    private void GenerateUI()
    {
        UIUtil.SetSize(container, 0.8f * _canvasSize.x, 0.5f * _canvasSize.y);

        UIUtil.SetSize(fadeBackground, _canvasSize);

        UIUtil.SetSizeKeepRatioY(iconBackgroundRT, 0.3f * container.sizeDelta.x);
        UIUtil.SetLocalPositionOfRectToAnotherRect(iconBackgroundRT, container, new Vector2(0.5f, -0.5f), new Vector2(-0.45f, 0.4f));

        UIUtil.SetSizeKeepRatioY(iconRT, 0.6f * iconBackgroundRT.sizeDelta.x);

        UIUtil.SetSizeX(equipmentNameRT, 0.5f * container.sizeDelta.x);
        UIUtil.SetLocalPositionOfRectToAnotherRect(equipmentNameRT, container, new Vector2(-0.5f, -0.5f), new Vector2(0.45f, 0.4f));
        UIUtil.SetLocalPositionY(equipmentNameRT, iconBackgroundRT.localPosition.y);

        UIUtil.SetSize(tagRarityRT, iconBackgroundRT.sizeDelta.x, 0.25f * iconBackgroundRT.sizeDelta.y);
        UIUtil.SetLocalPositionOfRectToAnotherRectVertically(tagRarityRT, iconBackgroundRT, 0.5f, -0.5f);
        UIUtil.SetLocalPositionX(tagRarityRT, equipmentNameRT.localPosition.x);

        UIUtil.SetSize(descriptionBackground, 0.9f * container.sizeDelta.x, 0.6f * container.sizeDelta.y);
        UIUtil.SetLocalPositionY(descriptionBackground,
            -0.5f * (container.sizeDelta.y - descriptionBackground.sizeDelta.y) + 0.05f * container.sizeDelta.x);

        UIUtil.SetSizeX(descriptionRT, 0.8f * container.sizeDelta.x);

        UIUtil.SetSize(closeButtonRT, 0.25f * container.sizeDelta.x, 0.1f * container.sizeDelta.x);
        UIUtil.SetLocalPositionOfRectToAnotherRect(closeButtonRT, container, -new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));

        UIUtil.SetSize(equipButtonRT, 0.5f * container.sizeDelta.x, 0.2f * container.sizeDelta.x);
        UIUtil.SetLocalPositionOfRectToAnotherRectVertically(equipButtonRT, container, -0.5f, -0.55f);

        UIUtil.SetFontSizeOnly(equipmentName, 0.025f * _canvasSize.y);
        UIUtil.SetFontSizeOnly(rarirtyText, 0.02f * _canvasSize.y);
        UIUtil.SetFontSizeOnly(description, 0.02f * _canvasSize.y);
        UIUtil.SetFontSizeOnly(equipButtonText, 0.02f * _canvasSize.y);
    }

    private void ShowAndRefresh(OwnedEquipmentData data, bool isEquip)
    {
        base.Show();

        icon.sprite = equipmentVisualProvider.EquipmentSprites[data.IconIndex]; ;

        equipmentName.text = data.Name;
        rarirtyText.text = $"{(RarityTier)(data.Rarity)}";
        description.text = data.GetSkill(skillContainer).GetDescription();

        Color rarityColor = ColorUtil.GetColorFromHex(SurvivoriumTheme.RARITY_COLORs[data.Rarity]);

        rarirtyText.color = rarityColor;
        rarirtyTag.color = rarityColor.Multiply(0.3f);

        _data = data;

        equipButton.onClick.RemoveAllListeners();

        if (isEquip)
        {
            equipButtonText.text = $"Equip";
            equipButton.onClick.AddListener(Equip);
        }
        else
        {
            equipButtonText.text = $"Unequip";
            equipButton.onClick.AddListener(Unequip);
        }
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }

    private void Equip()
    {
        equipmentSkillObserver.AddEquippedItem(_data);

        refreshCharacterScreenEvent?.Invoke();

        Close();
    }

    private void Unequip()
    {
        equipmentSkillObserver.UnequippedItem(_data);

        refreshCharacterScreenEvent?.Invoke();

        Close();
    }
}
