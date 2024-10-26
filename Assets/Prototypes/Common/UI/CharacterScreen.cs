using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterScreen : UIScreen
{
    [Header("UI ELEMENTS")]
    [SerializeField] private RectTransform slotPrefab;
    [SerializeField] private RectTransform slotContainer;

    [Header("CUSTOMIZE")]
    [SerializeField] private int numSlot;

    #region PRIVATE FIELD
    private RectTransform[] _slot;
    #endregion

    protected override void GenerateUI()
    {
        base.GenerateUI();

        _slot = new RectTransform[numSlot];

        for (int i = 0; i < numSlot; i++)
        {
            _slot[i] = Instantiate(slotPrefab, slotContainer);

            UIUtil.SetSize(_slot[i], 0.1f * _canvasSize.y, 0.1f * _canvasSize.y);

            if (i % 2 == 0)
            {
                UIUtil.SetLocalPositionX(_slot[i], 0.5f * (_canvasSize.x - _slot[i].sizeDelta.x) - SurvivoriumUIConstant.PADDING);
                UIUtil.SetLocalPositionY(_slot[i], 0.4f * _canvasSize.y - (i / 2) * 1.3f * _slot[i].sizeDelta.y);

                _UISlide.SlideIn(_slot[i], _slot[i].localPosition.x, 0.5f * (_canvasSize.x + _slot[i].sizeDelta.x));
            }
            else
            {
                UIUtil.SetLocalPositionX(_slot[i], -0.5f * (_canvasSize.x - _slot[i].sizeDelta.x) + SurvivoriumUIConstant.PADDING);
                UIUtil.SetLocalPositionY(_slot[i], 0.4f * _canvasSize.y - ((i - 1) / 2) * 1.3f * _slot[i].sizeDelta.y);

                _UISlide.SlideIn(_slot[i], _slot[i].localPosition.x, -0.5f * (_canvasSize.x + _slot[i].sizeDelta.x));
            }
        }
    }
}
