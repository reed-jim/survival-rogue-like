using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatDisplaySlot : MonoBehaviour
{
    [SerializeField] private RectTransform container;
    [SerializeField] private RectTransform iconBackground;
    [SerializeField] private RectTransform icon;
    [SerializeField] private RectTransform valueText;

    public void Setup(RectTransform parentContainer, int slotIndex)
    {
        GenerateUI(parentContainer, slotIndex);
    }

    private void GenerateUI(RectTransform parentContainer, int slotIndex)
    {
        UIUtil.SetSize(container, 0.4f * parentContainer.sizeDelta.x, 0.8f * parentContainer.sizeDelta.y);
        UIUtil.SetLocalPositionOfRectToAnotherRectHorizontally(container, parentContainer, -0.6f * (1 - 2 * slotIndex), 0);

        UIUtil.SetSizeKeepRatioX(iconBackground, 1.3f * container.sizeDelta.y);
        UIUtil.SetLocalPositionOfRectToAnotherRectHorizontally(iconBackground, container, 0, -0.5f);

        UIUtil.SetSizeKeepRatioX(icon, 0.5f * container.sizeDelta.y);

        UIUtil.SetSize(valueText, 0.5f * container.sizeDelta.x, 0.8f * container.sizeDelta.y);
        // UIUtil.SetLocalPositionOfRectToAnotherRectHorizontally(valueText, container, -0.4f, 0.4f);
    }
}
