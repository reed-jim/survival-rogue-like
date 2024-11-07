using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestOpeningScreen : UIScreen
{
    [SerializeField] private RectTransform sGradeChestContainer;
    [SerializeField] private RectTransform rareChestContainer;
    [SerializeField] private RectTransform commonChestContainer;

    [SerializeField] private RectTransform sGradeChestTitleRT;
    [SerializeField] private RectTransform rareChestTitleRT;
    [SerializeField] private RectTransform commonChestTitleRT;

    [SerializeField] private RectTransform sGradeChestOpenButtonRT;
    [SerializeField] private RectTransform rareChestOpenButtonRT;
    [SerializeField] private RectTransform commonChestOpenButtonRT;

    [SerializeField] private Button rareChestOpenButton;
    [SerializeField] private TMP_Text[] titleTexts;
    [SerializeField] private TMP_Text[] buttonTexts;

    [Header("DETAIL CHEST OPENNING")]
    [SerializeField] private ChestRevealScreen chestRevealScreen;

    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private SaferioTheme saferioTheme;

    public override void RegisterEvent()
    {
        base.RegisterEvent();

        rareChestOpenButton.onClick.AddListener(OpenRareChest);
    }

    protected override void GenerateUI()
    {
        float padding = 0.02f * _canvasSize.y;

        UIUtil.SetSize(sGradeChestContainer, _canvasSize.x - 2 * padding, 0.3f * _canvasSize.y);
        UIUtil.SetLocalPositionY(sGradeChestContainer, 0.3f * (_canvasSize.y - sGradeChestContainer.sizeDelta.y) - padding);

        UIUtil.SetSize(rareChestContainer, (_canvasSize.x - 3 * padding) / 2, 0.3f * _canvasSize.y);
        UIUtil.SetLocalPositionX(rareChestContainer, -0.5f * (_canvasSize.x - rareChestContainer.sizeDelta.x) + padding);
        UIUtil.SetLocalPositionY(rareChestContainer, sGradeChestContainer.localPosition.y - 0.5f * (sGradeChestContainer.sizeDelta.y + rareChestContainer.sizeDelta.y) - padding);

        UIUtil.SetSize(commonChestContainer, rareChestContainer.sizeDelta);
        UIUtil.SetLocalPositionX(commonChestContainer, 0.5f * (_canvasSize.x - commonChestContainer.sizeDelta.x) - padding);
        UIUtil.SetLocalPositionY(commonChestContainer, rareChestContainer.localPosition.y);

        UIUtil.SetLocalPositionOfRectToAnotherRectVertically(sGradeChestTitleRT, sGradeChestContainer, -0.5f, 0.4f);
        UIUtil.SetLocalPositionOfRectToAnotherRectVertically(rareChestTitleRT, rareChestContainer, -0.5f, 0.4f);
        UIUtil.SetLocalPositionOfRectToAnotherRectVertically(commonChestTitleRT, commonChestContainer, -0.5f, 0.4f);

        UIUtil.SetSizeKeepRatioY(sGradeChestOpenButtonRT, 0.6f * sGradeChestContainer.sizeDelta.x);
        UIUtil.SetSizeKeepRatioY(rareChestOpenButtonRT, 0.6f * rareChestContainer.sizeDelta.x);
        UIUtil.SetSizeKeepRatioY(commonChestOpenButtonRT, 0.6f * commonChestContainer.sizeDelta.x);
        UIUtil.SetLocalPositionOfRectToAnotherRectVertically(sGradeChestOpenButtonRT, sGradeChestContainer, 0.5f, -0.4f);
        UIUtil.SetLocalPositionOfRectToAnotherRectVertically(rareChestOpenButtonRT, rareChestContainer, 0.5f, -0.4f);
        UIUtil.SetLocalPositionOfRectToAnotherRectVertically(commonChestOpenButtonRT, commonChestContainer, 0.5f, -0.4f);

        UIUtil.SetSizeX(sGradeChestTitleRT, 0.8f * sGradeChestContainer.sizeDelta.x);
        UIUtil.SetSizeX(rareChestTitleRT, 0.8f * rareChestContainer.sizeDelta.x);
        UIUtil.SetSizeX(commonChestTitleRT, 0.8f * commonChestContainer.sizeDelta.x);

        for (int i = 0; i < titleTexts.Length; i++)
        {
            UIUtil.SetFontSize(titleTexts[i], 0.03f * _canvasSize.y);
            UIUtil.SetFontSize(buttonTexts[i], 0.03f * _canvasSize.y);

            titleTexts[i].color = saferioTheme.TitleColor;
            buttonTexts[i].color = saferioTheme.SecondaryTextColor;
        }
    }

    private void OpenRareChest()
    {
        chestRevealScreen.gameObject.SetActive(true);
    }
}
