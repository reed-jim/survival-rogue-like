using System.Collections;
using System.Collections.Generic;
using ReedJim.RPG.Stat;
using UnityEngine;

public class PermanentUpgradeScreen : UIScreen
{
    [SerializeField] private RectTransform upgradeSlotPrefab;
    [SerializeField] private RectTransform upgradeSlotContainer;

    [SerializeField] private PredifinedCharacterStat permanentUpgradeScriptable;
    [SerializeField] private CharacterStat permanentUpgradeStat;

    private int _slotCount;
    private RectTransform[] _upgradeSlots;
    private PermanentUpgradeSlot[] _permanentUpgradeSlots;

    private void Spawn()
    {
        permanentUpgradeStat = permanentUpgradeScriptable.GetBaseCharacterStat();
        _slotCount = permanentUpgradeStat.StatComponents.Count;

        _upgradeSlots = new RectTransform[_slotCount];
        _permanentUpgradeSlots = new PermanentUpgradeSlot[_slotCount];

        for (int i = 0; i < _slotCount; i++)
        {
            _upgradeSlots[i] = Instantiate(upgradeSlotPrefab, upgradeSlotContainer);

            _permanentUpgradeSlots[i] = _upgradeSlots[i].GetComponent<PermanentUpgradeSlot>();
        }
    }

    protected override void GenerateUI()
    {
        Spawn();

        UIUtil.SetSize(upgradeSlotContainer, 0.9f * _canvasSize.x, 0.2f * _canvasSize.y);
        UIUtil.SetLocalPositionY(upgradeSlotContainer, 0.4f * (_canvasSize.y - upgradeSlotContainer.sizeDelta.y));

        for (int i = 0; i < _permanentUpgradeSlots.Length; i++)
        {
            UIUtil.SetSizeKeepRatioY(_upgradeSlots[i], 0.9f * upgradeSlotContainer.sizeDelta.x / _slotCount);
            UIUtil.SetLocalPositionX(_upgradeSlots[i], -0.4f * upgradeSlotContainer.sizeDelta.x + 1.1f * i * _upgradeSlots[i].sizeDelta.x);
        }

        int slotIndex = 0;

        foreach (var item in permanentUpgradeStat.StatComponents.Values)
        {
            float statValue = item.BaseValue;

            _permanentUpgradeSlots[slotIndex].SetStatValue(statValue);

            slotIndex++;
        }
    }
}
