using System;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelingUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private RectTransform canvas;
    [SerializeField] private Button showUpgradePanelButton;
    private RectTransform _showUpgradePanelButtonRT;
    [SerializeField] private Button[] selectUpgradeButtons;
    private RectTransform[] selectUpgradeRTs;
    private TMP_Text[] skillNameTexts;
    private TMP_Text[] selectUpgradeTexts;
    private TMP_Text[] rarityTexts;
    [SerializeField] private Button rerollSkillButton;
    [SerializeField] private Image fadeBackground;

    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private SkillContainer skillContainer;

    [Header("MANAGEMENT")]
    private Vector2 _canvasSize;
    private ISkill[] _skillsToChoose;

    #region ACTION
    public static Action<ISkill> addSkillEvent;
    public static Action<bool> enableInput;
    #endregion

    #region LIFE CYCLE
    private void Awake()
    {
        StatManager.showUpgradePanelEvent += OnPlayerLeveledUp;

        _canvasSize = canvas.sizeDelta;

        _skillsToChoose = new ISkill[selectUpgradeButtons.Length];

        BuildUI();

        for (int i = 0; i < selectUpgradeButtons.Length; i++)
        {
            int slot = i;

            selectUpgradeButtons[i].onClick.AddListener(() => SelectUpgrade(slot));
        }

        showUpgradePanelButton.onClick.AddListener(ShowUpgradePanel);
        rerollSkillButton.onClick.AddListener(Reroll);

        fadeBackground.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        StatManager.showUpgradePanelEvent -= OnPlayerLeveledUp;
    }
    #endregion

    private void BuildUI()
    {
        selectUpgradeRTs = new RectTransform[selectUpgradeButtons.Length];
        skillNameTexts = new TMP_Text[selectUpgradeButtons.Length];
        selectUpgradeTexts = new TMP_Text[selectUpgradeButtons.Length];
        rarityTexts = new TMP_Text[selectUpgradeButtons.Length];

        _showUpgradePanelButtonRT = showUpgradePanelButton.GetComponent<RectTransform>();

        for (int i = 0; i < selectUpgradeButtons.Length; i++)
        {
            selectUpgradeRTs[i] = selectUpgradeButtons[i].GetComponent<RectTransform>();
            skillNameTexts[i] = selectUpgradeButtons[i].transform.GetChild(1).GetComponent<TMP_Text>();
            selectUpgradeTexts[i] = selectUpgradeButtons[i].transform.GetChild(2).GetComponent<TMP_Text>();
            rarityTexts[i] = selectUpgradeButtons[i].transform.GetChild(3).GetComponent<TMP_Text>();

            float distanceBetween = 0.02f * _canvasSize.x;

            UIUtil.SetSize(selectUpgradeRTs[i], (_canvasSize.x - (selectUpgradeButtons.Length + 1) * distanceBetween) / selectUpgradeButtons.Length, 0.5f * _canvasSize.y);

            // selectUpgradeRTs[i].sizeDelta =
            //     new Vector2((_canvasSize.x - (selectUpgradeButtons.Length + 1) * distanceBetween) / selectUpgradeButtons.Length, 0.5f * _canvasSize.y);
            selectUpgradeRTs[i].localPosition = new Vector2((i - 1) * (selectUpgradeRTs[i].sizeDelta.x + distanceBetween), 0);

            skillNameTexts[i].rectTransform.localPosition = new Vector2(0, 0.4f * selectUpgradeRTs[i].sizeDelta.y);
            rarityTexts[i].rectTransform.localPosition = new Vector2(0, 0.25f * selectUpgradeRTs[i].sizeDelta.y);

            UIUtil.SetSize(skillNameTexts[i].rectTransform, 0.9f * selectUpgradeRTs[i].sizeDelta.x, 0.2f * selectUpgradeRTs[i].sizeDelta.y);
            UIUtil.SetSize(selectUpgradeTexts[i].rectTransform, skillNameTexts[i].rectTransform.sizeDelta);
            UIUtil.SetSize(rarityTexts[i].rectTransform, skillNameTexts[i].rectTransform.sizeDelta);

            skillNameTexts[i].fontSize = 0.04f * _canvasSize.x;
            selectUpgradeTexts[i].fontSize = 0.03f * _canvasSize.x;
            rarityTexts[i].fontSize = 0.03f * _canvasSize.x;

            selectUpgradeRTs[i].gameObject.SetActive(false);
        }

        UIUtil.SetSize(fadeBackground.rectTransform, _canvasSize);

        _showUpgradePanelButtonRT.sizeDelta = new Vector2(0.2f * _canvasSize.y, 0.1f * _canvasSize.y);
        _showUpgradePanelButtonRT.localPosition = new Vector2(0, -0.35f * _canvasSize.y);

        showUpgradePanelButton.gameObject.SetActive(false);
    }

    private void SelectUpgrade(int slot)
    {
        addSkillEvent?.Invoke(_skillsToChoose[slot]);

        HideUpgradeSlot();
    }

    private void ShowUpgrades()
    {
        for (int i = 0; i < selectUpgradeTexts.Length; i++)
        {
            _skillsToChoose[i] = RandomSkillMachine.GetRandomSkill(skillContainer);

            // test purpose
            // _skillsToChoose[i] = skillContainer.AllSkills[i];

            skillNameTexts[i].text = _skillsToChoose[i].GetName();
            selectUpgradeTexts[i].text = _skillsToChoose[i].GetDescription();
            rarityTexts[i].text = ((RarityTier)_skillsToChoose[i].GetTier()).ToString();
        }
    }

    private void Reroll()
    {
        ShowUpgrades();
    }

    private void OnPlayerLeveledUp()
    {
        showUpgradePanelButton.gameObject.SetActive(true);
    }

    private void ShowUpgradePanel()
    {
        for (int i = 0; i < selectUpgradeButtons.Length; i++)
        {
            selectUpgradeRTs[i].gameObject.SetActive(true);
        }

        ShowUpgrades();

        showUpgradePanelButton.gameObject.SetActive(false);

        enableInput?.Invoke(false);

        Time.timeScale = 0;


        fadeBackground.gameObject.SetActive(true);
    }

    private void HideUpgradeSlot()
    {
        for (int i = 0; i < selectUpgradeButtons.Length; i++)
        {
            int index = i;
            Vector3 startPosition = selectUpgradeRTs[i].localPosition;

            Tween.LocalPositionY(selectUpgradeRTs[index], startPosition.y + _canvasSize.y, duration: 0.5f)
            .OnComplete(() =>
            {
                selectUpgradeRTs[index].localPosition = startPosition;
                selectUpgradeRTs[index].gameObject.SetActive(false);

                enableInput?.Invoke(true);
            });

            Time.timeScale = 1;
        }

        Tween.Color(fadeBackground, ColorUtil.WithAlpha(fadeBackground.color, 0), duration: 0.3f)
        .OnComplete(() => fadeBackground.gameObject.SetActive(false));
    }
}
