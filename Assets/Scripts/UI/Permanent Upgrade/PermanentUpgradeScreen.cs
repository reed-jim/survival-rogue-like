using System.Collections;
using System.Collections.Generic;
using ReedJim.RPG.Stat;
using UnityEngine;

public class PermanentUpgradeScreen : UIScreen
{
    [SerializeField] private RectTransform upgradeSlotPrefab;
    [SerializeField] private RectTransform upgradeSlotContainer;

    [SerializeField] private PredifinedCharacterStat permanentUpgradeScriptable;
    [SerializeField] private PermanentUpgradeStatObserver permanentUpgradeStatObserver;

    private int _slotCount;
    private RectTransform[] _upgradeSlots;
    private PermanentUpgradeSlot[] _permanentUpgradeSlots;

    private void Spawn()
    {
        permanentUpgradeStatObserver.PermanentUpgradeStat = permanentUpgradeScriptable.GetBaseCharacterStat();
        _slotCount = permanentUpgradeStatObserver.PermanentUpgradeStat.StatComponents.Count;

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

        UIUtil.SetSize(upgradeSlotContainer, 0.9f * _canvasSize.x, 0.5f * _canvasSize.y);
        UIUtil.SetLocalPositionY(upgradeSlotContainer, 0.4f * (_canvasSize.y - upgradeSlotContainer.sizeDelta.y));

        for (int i = 0; i < _permanentUpgradeSlots.Length; i++)
        {
            UIUtil.SetSizeKeepRatioY(_upgradeSlots[i], 0.3f * upgradeSlotContainer.sizeDelta.x);

            if (i % 2 == 0)
            {
                UIUtil.SetLocalPositionX(_upgradeSlots[i], -0.55f * _upgradeSlots[i].sizeDelta.x);
            }
            else
            {
                UIUtil.SetLocalPositionX(_upgradeSlots[i], 0.55f * _upgradeSlots[i].sizeDelta.x);
            }

            UIUtil.SetLocalPositionY(_upgradeSlots[i], 0.45f * (upgradeSlotContainer.sizeDelta.y - _upgradeSlots[i].sizeDelta.y) - (i / 2) * 1.1f * _upgradeSlots[i].sizeDelta.y);
        }

        int slotIndex = 0;

        foreach (var item in permanentUpgradeStatObserver.PermanentUpgradeStat.StatComponents)
        {
            _permanentUpgradeSlots[slotIndex].Setup(item.Key, item.Value);

            slotIndex++;
        }
    }
}
