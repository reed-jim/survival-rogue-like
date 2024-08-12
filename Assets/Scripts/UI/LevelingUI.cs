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
    private TMP_Text[] selectUpgradeTexts;

    [Header("MANAGEMENT")]
    private Vector2 _canvasSize;
    private StatType[] _upgradeStatTypes;
    private float[] _upgradeStatValues;

    #region ACTION
    public static Action<StatType, float> upgradePlayerStatEvent;
    public static Action<bool> enableInput;
    #endregion

    private void Awake()
    {
        StatManager.showUpgradePanelEvent += OnPlayerLeveledUp;

        _canvasSize = canvas.sizeDelta;

        _upgradeStatTypes = new StatType[selectUpgradeButtons.Length];
        _upgradeStatValues = new float[selectUpgradeButtons.Length];

        BuildUI();

        for (int i = 0; i < selectUpgradeButtons.Length; i++)
        {
            int slot = i;

            selectUpgradeButtons[i].onClick.AddListener(() => SelectUpgrade(slot));
        }

        showUpgradePanelButton.onClick.AddListener(ShowUpgradePanel);
    }

    private void OnDestroy()
    {
        StatManager.showUpgradePanelEvent -= OnPlayerLeveledUp;
    }

    private void BuildUI()
    {
        selectUpgradeRTs = new RectTransform[selectUpgradeButtons.Length];
        selectUpgradeTexts = new TMP_Text[selectUpgradeButtons.Length];

        _showUpgradePanelButtonRT = showUpgradePanelButton.GetComponent<RectTransform>();

        for (int i = 0; i < selectUpgradeButtons.Length; i++)
        {
            selectUpgradeRTs[i] = selectUpgradeButtons[i].GetComponent<RectTransform>();
            selectUpgradeTexts[i] = selectUpgradeButtons[i].transform.GetChild(0).GetComponent<TMP_Text>();

            selectUpgradeRTs[i].sizeDelta = new Vector2(0.25f * _canvasSize.x, 0.7f * _canvasSize.y);
            selectUpgradeRTs[i].localPosition = new Vector2((i - 1) * 1.1f * selectUpgradeRTs[i].sizeDelta.x, 0);

            selectUpgradeTexts[i].fontSize = 0.02f * _canvasSize.x;

            selectUpgradeRTs[i].gameObject.SetActive(false);
        }

        _showUpgradePanelButtonRT.sizeDelta = new Vector2(0.2f * _canvasSize.y, 0.1f * _canvasSize.y);
        _showUpgradePanelButtonRT.localPosition = new Vector2(0, -0.35f * _canvasSize.y);

        showUpgradePanelButton.gameObject.SetActive(false);
    }

    private void SelectUpgrade(int slot)
    {
        upgradePlayerStatEvent?.Invoke(_upgradeStatTypes[slot], _upgradeStatValues[slot]);

        HideUpgradeSlot();
    }

    private void ShowUpgrades()
    {
        for (int i = 0; i < selectUpgradeTexts.Length; i++)
        {
            selectUpgradeTexts[i].text = GetRandomUpgrade(slot: i);
        }
    }

    private string GetRandomUpgrade(int slot)
    {
        int statType = UnityEngine.Random.Range(0, 5);

        switch ((StatType)statType)
        {
            case StatType.HP: return GetHPUpgrade(slot);
            case StatType.DAMAGE: return GetDamageUpgrade(slot);
            case StatType.ATTACK_SPEED: return GetAttackSpeedUpgrade(slot);
            default: return GetDamageUpgrade(slot);
        }
    }

    private string GetHPUpgrade(int slot)
    {
        int additionalValue = 10 * UnityEngine.Random.Range(1, 5);

        _upgradeStatTypes[slot] = StatType.HP;
        _upgradeStatValues[slot] = additionalValue;

        return $"+{additionalValue} HP";
    }

    private string GetDamageUpgrade(int slot)
    {
        int additionalValue = 2 * UnityEngine.Random.Range(1, 30);

        _upgradeStatTypes[slot] = StatType.DAMAGE;
        _upgradeStatValues[slot] = additionalValue;

        return $"+{additionalValue} Base Damage";
    }

    private string GetAttackSpeedUpgrade(int slot)
    {
        float additionalValue = 0.01f * UnityEngine.Random.Range(1, 30);

        _upgradeStatTypes[slot] = StatType.ATTACK_SPEED;
        _upgradeStatValues[slot] = additionalValue;

        return $"+{additionalValue * 100}% Attack Speed";
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