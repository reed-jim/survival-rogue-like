using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreenWithGeneratedItems : UIScreen
{
    [Header("UI ELEMENTS")]
    [SerializeField] protected RectTransform slotPrefab;
    [SerializeField] protected RectTransform slotContainer;

    [Header("CUSTOMIZE")]
    [SerializeField] protected int numSlot;

    #region PRIVATE FIELD
    protected RectTransform[] _slot;
    #endregion

    protected override void InitVariable()
    {
        base.InitVariable();

        _slot = new RectTransform[numSlot];

        for (int i = 0; i < numSlot; i++)
        {
            _slot[i] = Instantiate(slotPrefab, slotContainer);
        }
    }
}
