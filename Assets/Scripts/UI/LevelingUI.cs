using System;
using PrimeTween;
using Saferio.Util.SaferioTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelingUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private RectTransform canvas;
    [SerializeField] private RectTransform container;
    [SerializeField] private RectTransform skillSlotPrefab;
    [SerializeField] private RectTransform rerollButtonRT;
    [SerializeField] private Button showUpgradePanelButton;
    private RectTransform _showUpgradePanelButtonRT;
    private SkillSelectionSlot[] _skillSelectionSlots;
    // [SerializeField] private Button[] selectUpgradeButtons;
    // private RectTransform[] selectUpgradeRTs;
    // private TMP_Text[] skillNameTexts;
    // private TMP_Text[] selectUpgradeTexts;
    // private TMP_Text[] rarityTexts;
    [SerializeField] private Button rerollSkillButton;
    [SerializeField] private Image fadeBackground;

    [Header("CUSTOMIZE")]
    [SerializeField] private int numSkillSlot;

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
        StatManager.showUpgradePanelEvent += ShowUpgradePanel;
        SkillSelectionSlot.closeSkillSelectionPopupEvent += HideSkillSelectionPopup;

        _canvasSize = canvas.sizeDelta;

        _skillsToChoose = new ISkill[numSkillSlot];

        BuildUI();

        showUpgradePanelButton.onClick.AddListener(ShowUpgradePanel);
        rerollSkillButton.onClick.AddListener(Reroll);

        fadeBackground.gameObject.SetActive(false);
    }

    private void Start()
    {
        SaferioTween.Delay(1f, onCompletedAction: () => ShowUpgradePanel());
    }

    private void OnDestroy()
    {
        StatManager.showUpgradePanelEvent -= ShowUpgradePanel;
        SkillSelectionSlot.closeSkillSelectionPopupEvent -= HideSkillSelectionPopup;
    }
    #endregion

    private void BuildUI()
    {
        _skillSelectionSlots = new SkillSelectionSlot[numSkillSlot];

        _showUpgradePanelButtonRT = showUpgradePanelButton.GetComponent<RectTransform>();

        for (int i = 0; i < numSkillSlot; i++)
        {
            RectTransform skillSlot = Instantiate(skillSlotPrefab, container);

            _skillSelectionSlots[i] = skillSlot.GetComponent<SkillSelectionSlot>();

            float distanceBetween = 0.02f * _canvasSize.x;

            UIUtil.SetSize(skillSlot, (_canvasSize.x - (numSkillSlot + 1) * distanceBetween) / numSkillSlot, 0.5f * _canvasSize.y);
            UIUtil.SetLocalPositionX(skillSlot, (i - 1) * (skillSlot.sizeDelta.x + distanceBetween));

            _skillSelectionSlots[i].GenerateUI();
        }

        UIUtil.SetSize(fadeBackground.rectTransform, _canvasSize);

        _showUpgradePanelButtonRT.sizeDelta = new Vector2(0.2f * _canvasSize.y, 0.1f * _canvasSize.y);
        _showUpgradePanelButtonRT.localPosition = new Vector2(0, -0.35f * _canvasSize.y);

        UIUtil.SetSize(rerollButtonRT, 0.2f * _canvasSize.y, 0.08f * _canvasSize.y);
        UIUtil.SetLocalPositionY(rerollButtonRT, -0.4f * _canvasSize.y);

        showUpgradePanelButton.gameObject.SetActive(false);
        container.gameObject.SetActive(false);
    }

    private void SelectUpgrade(int slot)
    {
        addSkillEvent?.Invoke(_skillsToChoose[slot]);

        HideSkillSelectionPopup();
    }

    private void ShowUpgrades()
    {
        ISkill[] randomSkills = RandomSkillMachine.GetThreeRandomSkills(skillContainer);

        for (int i = 0; i < randomSkills.Length; i++)
        {
            _skillsToChoose[i] = randomSkills[i];

            _skillSelectionSlots[i].Setup(_skillsToChoose[i]);
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
        container.gameObject.SetActive(true);

        ShowUpgrades();

        showUpgradePanelButton.gameObject.SetActive(false);

        enableInput?.Invoke(false);

        Time.timeScale = 0;

        rerollButtonRT.gameObject.SetActive(true);

        fadeBackground.gameObject.SetActive(true);
    }

    private void HideSkillSelectionPopup()
    {
        Vector3 startPosition = container.localPosition;

        Tween.LocalPositionY(container, startPosition.y + _canvasSize.y, duration: 0.5f)
        .OnComplete(() =>
        {
            container.localPosition = startPosition;
            container.gameObject.SetActive(false);

            enableInput?.Invoke(true);
        });

        Time.timeScale = 1;

        Tween.Color(fadeBackground, ColorUtil.WithAlpha(fadeBackground.color, 0), duration: 0.3f)
        .OnComplete(() => fadeBackground.gameObject.SetActive(false));
    }
}
