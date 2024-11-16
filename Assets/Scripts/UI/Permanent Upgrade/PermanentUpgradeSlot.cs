using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReedJim.RPG.Stat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PermanentUpgradeSlot : MonoBehaviour, ISaferioPageViewSlot
{
    [SerializeField] private RectTransform upgradeButtonRT;

    [SerializeField] private TMP_Text statNameText;
    [SerializeField] private TMP_Text statValueText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private Button upgradeButton;

    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private PermanentUpgradeStatObserver permanentUpgradeStatObserver;
    [SerializeField] private SaferioTheme saferioTheme;

    private string _statName;
    private float _statValue;

    public void Setup(string statName, BaseStatComponent statComponent)
    {
        // GenerateUI();

        upgradeButton.onClick.AddListener(Upgrade);

        statNameText.text = StringUtil.AddSpaceBeforeUppercase(statName);
        statValueText.text = $"{statComponent.BaseValue}";

        _statName = statName;
        _statValue = statComponent.BaseValue;
    }

    #region ISaferioPageViewSlot 
    public void Setup(int slotIndex)
    {
        GenerateUI();

        DelaySetup(slotIndex);
    }

    private async void DelaySetup(int slotIndex)
    {
        await WaitForCondition();

        CharacterStat stat = permanentUpgradeStatObserver.PermanentUpgradeStat;

        Dictionary<string, BaseStatComponent> statComponents = stat.StatComponents;

        if (slotIndex < statComponents.Count)
        {
            Setup(statComponents.ElementAt(slotIndex).Key, statComponents.ElementAt(slotIndex).Value);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    async Task WaitForCondition()
    {
        while (permanentUpgradeStatObserver.PermanentUpgradeStat == null)
        {
            await Task.Yield();
        }
    }
    #endregion

    private void GenerateUI()
    {
        RectTransform container = GetComponent<RectTransform>();

        UIUtil.SetSize(statNameText.rectTransform, 0.9f * container.sizeDelta);
        UIUtil.SetLocalPositionY(statNameText.rectTransform, 0.3f * container.sizeDelta.y);

        UIUtil.SetFontSizeOnly(statNameText, 0.1f * container.sizeDelta.y);
        UIUtil.SetFontSizeOnly(statValueText, 0.2f * container.sizeDelta.y);
        UIUtil.SetFontSizeOnly(costText, 0.1f * container.sizeDelta.y);

        UIUtil.SetSize(upgradeButtonRT, 1.1f * container.sizeDelta.x, 0.3f * container.sizeDelta.y);
        UIUtil.SetLocalPositionOfRectToAnotherRectVertically(upgradeButtonRT, container, 0.5f, -0.55f);

        statValueText.color = saferioTheme.TitleColor;
    }

    private void Upgrade()
    {
        permanentUpgradeStatObserver.PermanentUpgradeStat.SetStatBaseValue(_statName, _statValue + 999);
    }
}
