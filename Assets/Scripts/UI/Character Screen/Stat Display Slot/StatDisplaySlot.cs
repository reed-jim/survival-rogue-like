using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatDisplaySlot : MonoBehaviour
{
    [SerializeField] private RectTransform container;
    [SerializeField] private RectTransform iconBackground;
    [SerializeField] private RectTransform icon;
    [SerializeField] private RectTransform valueTextRT;

    [SerializeField] private TMP_Text valueText;

    [Header("DATA")]
    [SerializeField] private string statKey;

    private void Awake()
    {
        EquipmentSkillObserver.updateCharacterStatDisplayEvent += UpdateStatText;
    }

    private void OnDestroy()
    {
        EquipmentSkillObserver.updateCharacterStatDisplayEvent -= UpdateStatText;
    }

    public void Setup(RectTransform parentContainer, int slotIndex)
    {
        GenerateUI(parentContainer, slotIndex);
    }

    private void GenerateUI(RectTransform parentContainer, int slotIndex)
    {
        // UIUtil.SetSize(container, 0.3f * parentContainer.sizeDelta.x, 0.9f * parentContainer.sizeDelta.y);
        // UIUtil.SetLocalPositionOfRectToAnotherRectHorizontally(container, parentContainer, -0.5f * (1 - 2 * slotIndex), 0);

        UIUtil.SetSizeKeepRatioX(iconBackground, 1.3f * container.sizeDelta.y);
        UIUtil.SetLocalPositionX(iconBackground, -0.15f * container.sizeDelta.x);

        UIUtil.SetSizeKeepRatioX(icon, 0.5f * container.sizeDelta.y);

        UIUtil.SetSize(valueTextRT, 0.5f * container.sizeDelta.x, 0.8f * container.sizeDelta.y);
        UIUtil.SetLocalPositionX(valueTextRT, 0.15f * container.sizeDelta.x);
        // UIUtil.SetLocalPositionOfRectToAnotherRectHorizontally(valueText, container, -0.4f, 0.4f);
    }

    private void UpdateStatText(string statKey, float value)
    {
        if (statKey == this.statKey)
        {
            valueText.text = $"{(int)value}";
        }
    }
}
