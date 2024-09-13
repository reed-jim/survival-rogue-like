using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBlock : MonoBehaviour
{
    #region ACTION
    public static event Action<int, int> showPointTextEvent;
    #endregion

    protected void InvokeShowPointTextEvent(int point)
    {
        showPointTextEvent?.Invoke(gameObject.GetInstanceID(), point);
    }
}
