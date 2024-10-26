using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpeningScreen : UIScreen
{
    [SerializeField] private RectTransform sGradeChestContainer;
    [SerializeField] private RectTransform rareChestContainer;
    [SerializeField] private RectTransform commonChestContainer;

    protected override void GenerateUI()
    {
        float padding = 0.05f * _canvasSize.y;

        UIUtil.SetSize(sGradeChestContainer, _canvasSize.x - 2 * padding, 0.3f * _canvasSize.y);
        UIUtil.SetLocalPositionY(sGradeChestContainer, 0.5f * (_canvasSize.y - sGradeChestContainer.sizeDelta.y) - padding);

        UIUtil.SetSize(rareChestContainer, (_canvasSize.x - 3 * padding) / 2, 0.3f * _canvasSize.y);
        UIUtil.SetLocalPositionX(rareChestContainer, -0.5f * (_canvasSize.x - rareChestContainer.sizeDelta.x) + padding);
        UIUtil.SetLocalPositionY(rareChestContainer, -0.5f * (_canvasSize.y - rareChestContainer.sizeDelta.y) + padding);

        UIUtil.SetSize(commonChestContainer, rareChestContainer.sizeDelta);
        UIUtil.SetLocalPositionX(commonChestContainer, 0.5f * (_canvasSize.x - commonChestContainer.sizeDelta.x) - padding);
        UIUtil.SetLocalPositionY(commonChestContainer, rareChestContainer.localPosition.y);
    }
}
