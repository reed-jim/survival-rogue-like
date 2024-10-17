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
    [SerializeField] private RectTransform descriptionRT;
    [SerializeField] private RectTransform closeButtonRT;

    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text equipmentName;
    [SerializeField] private TMP_Text description;
    [SerializeField] private Button closeButton;

    private Vector2 _canvasSize;

    private void Awake()
    {
        EquipmentSlot.openEquipmentDetailEvent += Show;

        _canvasSize = canvas.sizeDelta;

        closeButton.onClick.AddListener(Close);

        GenerateUI();

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

    private void GenerateUI()
    {
        UIUtil.SetSize(container, 0.7f * _canvasSize.x, 0.5f * _canvasSize.y);

        UIUtil.SetSize(fadeBackground, _canvasSize);

        UIUtil.SetSizeKeepRatioY(iconBackgroundRT, 0.3f * container.sizeDelta.x);
        UIUtil.SetLocalPositionOfRectToAnotherRect(iconBackgroundRT, container, new Vector2(0.5f, -0.5f), new Vector2(-0.45f, 0.4f));

        UIUtil.SetSizeKeepRatioY(iconRT, 0.6f * iconBackgroundRT.sizeDelta.x);

        UIUtil.SetSizeX(equipmentNameRT, 0.5f * container.sizeDelta.x);
        UIUtil.SetLocalPositionOfRectToAnotherRect(equipmentNameRT, container, new Vector2(-0.5f, -0.5f), new Vector2(0.45f, 0.4f));
        UIUtil.SetLocalPositionY(equipmentNameRT, iconBackgroundRT.localPosition.y);

        UIUtil.SetSizeX(descriptionRT, 0.8f * container.sizeDelta.x);
        UIUtil.SetLocalPositionY(descriptionRT, -0.1f * container.sizeDelta.y);

        UIUtil.SetSizeKeepRatioY(closeButtonRT, 0.15f * container.sizeDelta.x);
        UIUtil.SetLocalPositionOfRectToAnotherRect(closeButtonRT, container, Vector2.zero, new Vector2(0.5f, 0.5f));
    }

    private void Show(EquipmentSlotData data)
    {
        gameObject.SetActive(true);

        icon.sprite = data.Icon;

        equipmentName.text = data.name;

        description.text = data.Skill.GetDescription();
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }
}
