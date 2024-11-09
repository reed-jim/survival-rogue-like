using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterScreen : UIScreen
{
    [Header("UI ELEMENTS")]
    [SerializeField] private RectTransform slotPrefab;
    [SerializeField] private RectTransform slotContainer;

    [SerializeField] private RectTransform equipmentContainer;
    [SerializeField] private RectTransform statDisplayContainer;
    [SerializeField] private RectTransform inventoryContainer;
    [SerializeField] private RectTransform attackDisplaySlotRT;
    [SerializeField] private RectTransform hpDisplaySlotRT;

    [SerializeField] private StatDisplaySlot attackDisplaySlot;
    [SerializeField] private StatDisplaySlot hpDisplaySlot;

    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private EquipmentSkillObserver equipmentSkillObserver;

    [Header("CUSTOMIZE")]
    [SerializeField] private int numRow = 3;
    [SerializeField] private int numSlot;

    #region PRIVATE FIELD
    private RectTransform[] _slot;
    #endregion

    protected override void GenerateUI()
    {
        base.GenerateUI();

        _slot = new RectTransform[numSlot];

        float padding = SurvivoriumUIConstant.PADDING;

        UIUtil.SetSize(equipmentContainer, _canvasSize.x - 2 * padding, 0.35f * _canvasSize.y);
        UIUtil.SetSize(statDisplayContainer, equipmentContainer.sizeDelta.x, 0.05f * _canvasSize.y);
        UIUtil.SetSize(inventoryContainer, equipmentContainer.sizeDelta);

        UIUtil.SetLocalPositionY(equipmentContainer, 0.5f * (_canvasSize.y - equipmentContainer.sizeDelta.y) - padding);
        UIUtil.SetLocalPositionY(statDisplayContainer, equipmentContainer.localPosition.y - 0.5f * (statDisplayContainer.sizeDelta.y + equipmentContainer.sizeDelta.y) - padding);
        UIUtil.SetLocalPositionY(inventoryContainer, statDisplayContainer.localPosition.y - 0.5f * (inventoryContainer.sizeDelta.y + statDisplayContainer.sizeDelta.y) - padding);

        UIUtil.SetSize(attackDisplaySlotRT, 0.3f * statDisplayContainer.sizeDelta.x, statDisplayContainer.sizeDelta.y);
        UIUtil.SetSize(hpDisplaySlotRT, 0.3f * statDisplayContainer.sizeDelta.x, statDisplayContainer.sizeDelta.y);
        UIUtil.SetLocalPositionOfRectToAnotherRectHorizontally(attackDisplaySlotRT, statDisplayContainer, 0.5f, -0.5f);
        UIUtil.SetLocalPositionOfRectToAnotherRectHorizontally(hpDisplaySlotRT, statDisplayContainer, -0.5f, 0.5f);

        float distanceBetweenSlot = 0.05f * equipmentContainer.sizeDelta.y;
        float slotSize = (equipmentContainer.sizeDelta.y - (numRow - 1) * distanceBetweenSlot - 2 * SurvivoriumUIConstant.PADDING) / numRow;

        for (int i = 0; i < numSlot; i++)
        {
            _slot[i] = Instantiate(slotPrefab, slotContainer);

            UIUtil.SetSize(_slot[i], slotSize * Vector3.one);

            int yIndex = (i - (i % 2)) / 2;

            if (i % 2 == 0)
            {
                UIUtil.SetLocalPositionX(_slot[i], -0.5f * (equipmentContainer.sizeDelta.x - slotSize) + SurvivoriumUIConstant.PADDING);
                UIUtil.SetLocalPositionY(_slot[i], ((numRow - 1) / 2 - yIndex) * (slotSize + distanceBetweenSlot));

                _UISlide.SlideIn(_slot[i], _slot[i].localPosition.x, 0.5f * (_canvasSize.x + slotSize));
            }
            else
            {
                UIUtil.SetLocalPositionX(_slot[i], 0.5f * (equipmentContainer.sizeDelta.x - slotSize) - SurvivoriumUIConstant.PADDING);
                UIUtil.SetLocalPositionY(_slot[i], ((numRow - 1) / 2 - yIndex) * (slotSize + distanceBetweenSlot));
                // UIUtil.SetLocalPositionY(_slot[i], 0.5f * equipmentContainer.sizeDelta.y - ((i - 1) / 2 + 0.5f) * 1.3f * _slot[i].sizeDelta.y);

                _UISlide.SlideIn(_slot[i], _slot[i].localPosition.x, -0.5f * (_canvasSize.x + slotSize));
            }
        }

        // List<OwnedEquipmentData> ownedEquipmentsData = equipmentSkillObserver.OwnedItemDatum;

        // for (int i = 0; i < ownedEquipmentsData.Count; i++)
        // {
        //     _slot[i].GetComponent<EquippedItemSlot>().Setup(ownedEquipmentsData[i]);
        // }

        attackDisplaySlot.Setup(statDisplayContainer, 0);
        hpDisplaySlot.Setup(statDisplayContainer, 1);
    }

    private void Reset()
    {

    }
}
