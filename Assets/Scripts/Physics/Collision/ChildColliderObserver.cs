using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildColliderObserver : MonoBehaviour
{
    #region ACTION
    private event Action<Collider> _onTriggerEnterEvent;
    #endregion

    private void OnDestroy()
    {
        UnregisterOnTriggerEvent();
    }

    private void OnTriggerEnter(Collider other)
    {
        _onTriggerEnterEvent?.Invoke(other);
    }

    public void RegisterOnTriggerEvent(Action<Collider> test)
    {
        _onTriggerEnterEvent += test;
    }

    public void UnregisterOnTriggerEvent()
    {
        _onTriggerEnterEvent = null;
    }
}
