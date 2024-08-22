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

        Tween.Delay(1).OnComplete(() => ShowUpgradePanel());
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

        _showUpgradePanelButtonRT = showUpgradePanelButton.GetComponent<RectTransform>();

        for (int i = 0; i < selectUpgradeButtons.Length; i++)
        {
            selectUpgradeRTs[i] = selectUpgradeButtons[i].GetComponent<RectTransform>();
            skillNameTexts[i] = selectUpgradeButtons[i].transform.GetChild(0).GetComponent<TMP_Text>();
            selectUpgradeTexts[i] = selectUpgradeButtons[i].transform.GetChild(1).GetComponent<TMP_Text>();

            selectUpgradeRTs[i].sizeDelta = new Vector2(0.25f * _canvasSize.x, 0.7f * _canvasSize.y);
            selectUpgradeRTs[i].localPosition = new Vector2((i - 1) * 1.1f * selectUpgradeRTs[i].sizeDelta.x, 0);

            skillNameTexts[i].rectTransform.localPosition = new Vector2(0, 0.35f * selectUpgradeRTs[i].sizeDelta.y);

            skillNameTexts[i].fontSize = 0.02f * _canvasSize.x;
            selectUpgradeTexts[i].fontSize = 0.02f * _canvasSize.x;

            selectUpgradeRTs[i].gameObject.SetActive(false);
        }

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
            // _skillsToChoose[i] = RandomSkillMachine.GetRandomSkill(skillContainer);
            _skillsToChoose[i] = skillContainer.AllSkills[5 + i];

            skillNameTexts[i].text = _skillsToChoose[i].GetName();
            selectUpgradeTexts[i].text = _skillsToChoose[i].GetDescription();
        }
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
    }
}
