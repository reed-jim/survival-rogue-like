using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelectionSlot : MonoBehaviour
{
    [SerializeField] private RectTransform container;
    [SerializeField] private RectTransform skillNameBackground;
    [SerializeField] private RectTransform skillNameRT;
    [SerializeField] private RectTransform rarityTextRT;
    [SerializeField] private RectTransform descriptionRT;

    [SerializeField] private Image skillNameBackgroundImage;
    [SerializeField] private Image skillImage;
    [SerializeField] private TMP_Text skillNameText;
    [SerializeField] private TMP_Text rarityText;
    [SerializeField] private Image rarityGradient;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Button selectSkillButton;

    #region ACTION
    public static Action<ISkill> addSkillEvent;
    public static Action closeSkillSelectionPopupEvent;
    #endregion

    public void Setup(ISkill skill)
    {
        int rarityTier = skill.GetTier();

        RarityTier rarity = (RarityTier)rarityTier;

        string colorHex = SurvivoriumTheme.RARITY_COLORs[rarityTier];

        string rarityString = $"<color={SurvivoriumTheme.RARITY_COLORs[rarityTier]}>{rarity}</color>";

        skillNameText.text = skill.GetName();
        rarityText.text = rarityString;
        descriptionText.text = skill.GetDescription();

        skillNameBackgroundImage.color = ColorUtil.GetColorFromHex(colorHex.Replace("#", ""));
        rarityGradient.color = ColorUtil.GetColorFromHex(colorHex.Replace("#", ""));

        selectSkillButton.onClick.RemoveAllListeners();
        selectSkillButton.onClick.AddListener(() => SelectSkill(skill));
    }

    public void GenerateUI()
    {
        UIUtil.SetSize(skillNameBackground, container.sizeDelta.x, 0.15f * container.sizeDelta.y);
        UIUtil.SetLocalPositionOfRectToAnotherRectVertically(skillNameBackground, container, -0.5f, 0.5f);

        // UIUtil.SetSizeKeepRatioX(skillImage, 0.2f * container.sizeDelta.y);
        // UIUtil.SetLocalPositionY(skillImage, 0.1f * container.sizeDelta.y);

        rarityText.fontSize = 0.035f * container.sizeDelta.y;
        UIUtil.SetSize(rarityTextRT, 0.9f * container.sizeDelta.x, 0.1f * container.sizeDelta.y);
        UIUtil.SetLocalPositionOfRectToAnotherRectVertically(rarityTextRT, skillNameBackground, -0.6f, -0.5f);

        skillNameText.fontSize = 0.04f * container.sizeDelta.y;
        UIUtil.SetSize(skillNameRT, 0.9f * container.sizeDelta);

        descriptionText.fontSize = 0.035f * container.sizeDelta.y;
        UIUtil.SetSize(descriptionRT, 0.9f * container.sizeDelta.x, 0.5f * container.sizeDelta.y);
        UIUtil.SetLocalPositionOfRectToAnotherRectVertically(descriptionRT, container, 0.5f, -0.45f);
    }

    private void SelectSkill(ISkill skill)
    {
        addSkillEvent?.Invoke(skill);
        closeSkillSelectionPopupEvent?.Invoke();
    }
}
