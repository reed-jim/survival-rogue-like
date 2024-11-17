using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    private EquippedItemSlot[] _equippedItemSlots;
    #endregion

    public override void RegisterEvent()
    {
        base.RegisterEvent();

        EquipmentDetail.refreshCharacterScreenEvent += UpdateState;
    }

    public override void UnregisterEvent()
    {
        base.UnregisterEvent();

        EquipmentDetail.refreshCharacterScreenEvent -= UpdateState;
    }

    public override void UpdateState()
    {
        if (equipmentSkillObserver.EquippedItemDatum == null)
        {
            return;
        }

        List<OwnedEquipmentData> equipmentItemDatum = equipmentSkillObserver.EquippedItemDatum;

        for (int i = 0; i < equipmentItemDatum.Count; i++)
        {
            _equippedItemSlots[i].Setup(equipmentItemDatum[i]);
        }

        for (int i = 0; i < numSlot; i++)
        {
            if (i >= equipmentItemDatum.Count)
            {
                _equippedItemSlots[i].Unequip();
            }
        }
    }

    protected override void GenerateUI()
    {
        base.GenerateUI();

        _slot = new RectTransform[numSlot];
        _equippedItemSlots = new EquippedItemSlot[numSlot];

        float padding = SurvivoriumUIConstant.PADDING;

        UIUtil.SetSize(equipmentContainer, _canvasSize.x - 2 * padding, 0.35f * _canvasSize.y);
        UIUtil.SetSize(statDisplayContainer, equipmentContainer.sizeDelta.x, 0.05f * _canvasSize.y);
        UIUtil.SetSize(inventoryContainer, _canvasSize.x, 0.4f * _canvasSize.y);

        // UIUtil.SetLocalPositionY(equipmentContainer, 0.5f * (_canvasSize.y - equipmentContainer.sizeDelta.y) - padding);
        // UIUtil.SetLocalPositionY(statDisplayContainer, equipmentContainer.localPosition.y - 0.5f * (statDisplayContainer.sizeDelta.y + equipmentContainer.sizeDelta.y) - padding);
        // UIUtil.SetLocalPositionY(inventoryContainer, statDisplayContainer.localPosition.y - 0.5f * (inventoryContainer.sizeDelta.y + statDisplayContainer.sizeDelta.y) - padding);
        UIUtil.SetLocalPositionOfRectToAnotherRectVertically(inventoryContainer, canvas, 0.5f, -0.5f);
        UIUtil.SetLocalPositionY(statDisplayContainer, inventoryContainer.localPosition.y + 0.5f * (inventoryContainer.sizeDelta.y + statDisplayContainer.sizeDelta.y) + padding);
        UIUtil.SetLocalPositionY(equipmentContainer, statDisplayContainer.localPosition.y + 0.5f * (statDisplayContainer.sizeDelta.y + equipmentContainer.sizeDelta.y) + padding);

        UIUtil.SetSize(attackDisplaySlotRT, 0.3f * statDisplayContainer.sizeDelta.x, statDisplayContainer.sizeDelta.y);
        UIUtil.SetSize(hpDisplaySlotRT, 0.3f * statDisplayContainer.sizeDelta.x, statDisplayContainer.sizeDelta.y);
        UIUtil.SetLocalPositionOfRectToAnotherRectHorizontally(attackDisplaySlotRT, statDisplayContainer, 0.5f, -0.5f);
        UIUtil.SetLocalPositionOfRectToAnotherRectHorizontally(hpDisplaySlotRT, statDisplayContainer, -0.5f, 0.5f);

        float distanceBetweenSlot = 0.08f * equipmentContainer.sizeDelta.y;
        float slotSize = (equipmentContainer.sizeDelta.y - (numRow - 1) * distanceBetweenSlot - 2 * SurvivoriumUIConstant.PADDING) / numRow;

        for (int i = 0; i < numSlot; i++)
        {
            _slot[i] = Instantiate(slotPrefab, slotContainer);
            _equippedItemSlots[i] = _slot[i].GetComponent<EquippedItemSlot>();

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

            _equippedItemSlots[i].GenerateUI();
        }

        attackDisplaySlot.Setup(statDisplayContainer, 0);
        hpDisplaySlot.Setup(statDisplayContainer, 1);
    }
}
