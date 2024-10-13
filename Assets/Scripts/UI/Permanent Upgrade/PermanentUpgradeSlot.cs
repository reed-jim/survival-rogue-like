using System.Collections;
using System.Collections.Generic;
using ReedJim.RPG.Stat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PermanentUpgradeSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text statNameText;
    [SerializeField] private TMP_Text statValueText;
    [SerializeField] private Button upgradeButton;

    [SerializeField] private PermanentUpgradeStatObserver permanentUpgradeStatObserver;

    private string _statName;
    private float _statValue;

    public void Setup(string statName, BaseStatComponent statComponent)
    {
        GenerateUI();

        upgradeButton.onClick.AddListener(Upgrade);

        statNameText.text = statName;
        statValueText.text = $"{statComponent.BaseValue}";

        _statName = statName;
        _statValue = statComponent.BaseValue;
    }

    private void GenerateUI()
    {
        RectTransform container = GetComponent<RectTransform>();

        UIUtil.SetSize(statNameText.rectTransform, 0.9f * container.sizeDelta);
        UIUtil.SetLocalPositionY(statNameText.rectTransform, 0.35f * container.sizeDelta.y);
    }

    private void Upgrade()
    {
        permanentUpgradeStatObserver.PermanentUpgradeStat.SetStatBaseValue(_statName, _statValue + 999);
    }
}
