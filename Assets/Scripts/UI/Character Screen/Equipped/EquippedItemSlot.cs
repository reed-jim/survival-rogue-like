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
    [SerializeField] private RectTransform levelTextRT;

    [SerializeField] private Image containerBackground;
    [SerializeField] private Image icon;
    [SerializeField] private Button selectButton;
    [SerializeField] private TMP_Text levelText;

    [SerializeField] private EquipmentSlotDataContainer equipmentSlotDataContainer;

    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private EquipmentVisualProvider equipmentVisualProvider;

    #region PRIVATE FIELD
    private EquipmentSlotData _equipmentSlotData;
    #endregion

    #region ACTION
    public static event Action<EquipmentSlotData> openEquipmentDetailEvent;
    #endregion

    private void Awake()
    {
        icon.gameObject.SetActive(false);

        RegisterButton();

        GenerateUI();
    }

    public void Setup(OwnedEquipmentData equipmentData)
    {
        icon.gameObject.SetActive(true);
        icon.sprite = equipmentVisualProvider.EquipmentSprites[equipmentData.IconIndex];

        containerBackground.color = ColorUtil.GetColorFromHex(SurvivoriumTheme.RARITY_COLORs[equipmentData.Rarity]);

        levelText.text = $"Lv {equipmentData.Level}";

        levelTextRT.gameObject.SetActive(true);
    }

    private void RegisterButton()
    {
        selectButton.onClick.AddListener(Select);
    }

    private void GenerateUI()
    {
        float padding = 0.2f * container.sizeDelta.y;

        UIUtil.SetLocalPosition(levelTextRT, 0.5f * container.sizeDelta - padding * Vector2.one);
        
        levelTextRT.gameObject.SetActive(false);
    }

    private void Select()
    {
        openEquipmentDetailEvent?.Invoke(_equipmentSlotData);
    }
}
