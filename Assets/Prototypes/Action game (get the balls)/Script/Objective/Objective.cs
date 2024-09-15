using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    #region ACTION
    public static event Action pickObjectiveEvent;
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        pickObjectiveEvent?.Invoke();
    }
}
