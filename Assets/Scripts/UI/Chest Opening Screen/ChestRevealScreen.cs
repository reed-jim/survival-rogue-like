using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestRevealScreen : MonoBehaviour
{
    [SerializeField] private RectTransform canvas;
    [SerializeField] private RectTransform container;
    [SerializeField] private RectTransform chest;
    [SerializeField] private RectTransform itemRT;
    [SerializeField] private RectTransform itemNameTextRT;
    [SerializeField] private RectTransform rarityTextRT;

    [SerializeField] private Image item;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text rarityText;
    [SerializeField] private Button continueButton;

    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private EquipmentPool equipmentPool;
    [SerializeField] private EquipmentVisualProvider equipmentVisualProvider;
    [SerializeField] private EquipmentSkillObserver equipmentSkillObserver;
    [SerializeField] private SkillContainer skillContainer;

    private Vector2 _canvasSize;

    private void Awake()
    {
        _canvasSize = canvas.sizeDelta;

        GenerateUI();

        continueButton.onClick.AddListener(Close);
    }

    private void OnEnable()
    {
        OpenChest();
    }

    private void GenerateUI()
    {
        UIUtil.SetSize(container, _canvasSize);

        UIUtil.SetSizeKeepRatioX(chest, 0.2f * _canvasSize.y);
        UIUtil.SetLocalPositionY(chest, -0.15f * _canvasSize.y);

        UIUtil.SetSizeKeepRatioX(itemRT, 0.1f * _canvasSize.y);
        UIUtil.SetLocalPositionY(itemRT, 0.2f * _canvasSize.y);

        UIUtil.SetSize(itemNameTextRT, 0.8f * _canvasSize.x, 0.05f * _canvasSize.y);
        UIUtil.SetSize(rarityTextRT, 0.8f * _canvasSize.x, 0.05f * _canvasSize.y);
        UIUtil.SetLocalPositionOfRectToAnotherRectVertically(itemNameTextRT, itemRT, 0.6f, 0.6f);
        UIUtil.SetLocalPositionOfRectToAnotherRectVertically(rarityTextRT, itemNameTextRT, 0.6f, 0.6f);

        UIUtil.SetLocalPositionOfRectToAnotherRectVertically(continueButton, container, 0.5f, -0.4f);
    }

    private void OpenChest()
    {
        OwnedEquipmentData ownedEquipmentData = new OwnedEquipmentData();

        int randomIndex = Random.Range(0, equipmentPool.EquipmentsData.Length);

        EquipmentData randomEquipment = equipmentPool.EquipmentsData[randomIndex];

        ownedEquipmentData.Name = randomEquipment.EquipmentName;
        ownedEquipmentData.IconIndex = randomEquipment.IconIndex;
        ownedEquipmentData.SkillIndexInContainer = skillContainer.GetSkillIndex(randomEquipment.Skill);
        ownedEquipmentData.Rarity = Random.Range(0, 5);
        ownedEquipmentData.Level = 1;

        UpdateUI(ownedEquipmentData);

        equipmentSkillObserver.AddOwnedItem(ownedEquipmentData);
    }

    private void UpdateUI(OwnedEquipmentData data)
    {
        item.sprite = equipmentVisualProvider.EquipmentSprites[data.IconIndex];

        UIUtil.SetSizeKeepRatioX(item, 0.1f * _canvasSize.y);

        itemNameText.text = $"{data.Name}";
        rarityText.text = $"{(RarityTier)data.Rarity}";
    }

    private void Close()
    {
        container.gameObject.SetActive(false);
    }
}
