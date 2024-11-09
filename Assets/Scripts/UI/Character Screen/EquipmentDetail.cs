using System.Collections;
using System.Collections.Generic;
using ReedJim.RPG.Stat;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentDetail : MonoBehaviour
{
    [SerializeField] private RectTransform canvas;
    [SerializeField] private RectTransform container;
    [SerializeField] private RectTransform fadeBackground;
    [SerializeField] private RectTransform iconBackgroundRT;
    [SerializeField] private RectTransform iconRT;
    [SerializeField] private RectTransform equipmentNameRT;
    [SerializeField] private RectTransform descriptionBackground;
    [SerializeField] private RectTransform descriptionRT;
    [SerializeField] private RectTransform closeButtonRT;
    [SerializeField] private RectTransform equipButtonRT;

    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text equipmentName;
    [SerializeField] private TMP_Text description;
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

    private void Awake()
    {
        EquipmentSlot.openEquipmentDetailEvent += Show;

        _canvasSize = canvas.sizeDelta;

        RegisterButton();

        GenerateUI();

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EquipmentSlot.openEquipmentDetailEvent -= Show;
    }

    public void Setup(OwnedEquipmentData data)
    {
        icon.sprite = equipmentVisualProvider.EquipmentSprites[data.IconIndex];
    }

    private void RegisterButton()
    {
        closeButton.onClick.AddListener(Close);
        equipButton.onClick.AddListener(Equip);
    }

    private void GenerateUI()
    {
        UIUtil.SetSize(container, 0.75f * _canvasSize.x, 0.6f * _canvasSize.y);

        UIUtil.SetSize(fadeBackground, _canvasSize);

        UIUtil.SetSizeKeepRatioY(iconBackgroundRT, 0.3f * container.sizeDelta.x);
        UIUtil.SetLocalPositionOfRectToAnotherRect(iconBackgroundRT, container, new Vector2(0.5f, -0.5f), new Vector2(-0.45f, 0.4f));

        UIUtil.SetSizeKeepRatioY(iconRT, 0.6f * iconBackgroundRT.sizeDelta.x);

        UIUtil.SetSizeX(equipmentNameRT, 0.5f * container.sizeDelta.x);
        UIUtil.SetLocalPositionOfRectToAnotherRect(equipmentNameRT, container, new Vector2(-0.5f, -0.5f), new Vector2(0.45f, 0.4f));
        UIUtil.SetLocalPositionY(equipmentNameRT, iconBackgroundRT.localPosition.y);

        UIUtil.SetSize(descriptionBackground, 0.9f * container.sizeDelta.x, 0.6f * container.sizeDelta.y);
        UIUtil.SetLocalPositionOfRectToAnotherRectVertically(descriptionBackground, container, 0.5f, -0.45f);
        UIUtil.SetLocalPositionX(descriptionBackground, 0);

        UIUtil.SetSizeX(descriptionRT, 0.8f * container.sizeDelta.x);

        UIUtil.SetSize(closeButtonRT, 0.25f * container.sizeDelta.x, 0.1f * container.sizeDelta.x);
        UIUtil.SetLocalPositionOfRectToAnotherRect(closeButtonRT, container, -new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));

        UIUtil.SetSize(equipButtonRT, 0.5f * container.sizeDelta.x, 0.2f * container.sizeDelta.x);
        UIUtil.SetLocalPositionOfRectToAnotherRectVertically(equipButtonRT, container, 0, -0.5f);
    }

    private void Show(OwnedEquipmentData data)
    {
        gameObject.SetActive(true);

        icon.sprite = equipmentVisualProvider.EquipmentSprites[data.IconIndex]; ;

        equipmentName.text = data.Name;

        description.text = data.GetSkill(skillContainer).GetDescription();

        _data = data;
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }

    private void Equip()
    {
        equipmentSkillObserver.AddEquippedItem(_data);
    }
}
