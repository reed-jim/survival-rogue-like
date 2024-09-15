using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskScreen : UIScreenWithGeneratedItems
{
    [Header("DATA")]
    [SerializeField] private TaskDataContainer taskDataContainer;

    #region PRIVATE FIELD
    
    #endregion
    
    protected override void GenerateUI()
    {
        for (int i = 0; i < numSlot; i++)
        {
            UIUtil.SetSize(_slot[i], 0.8f * _canvasSize.x, 0.15f * _canvasSize.y);
            UIUtil.SetLocalPositionY(_slot[i], ((numSlot - 1) / 2 - i) * 1.1f * _slot[i].sizeDelta.y);
        }
    }
}
