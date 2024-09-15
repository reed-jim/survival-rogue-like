using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterObjectiveHolder : MonoBehaviour
{
    #region PRIVATE FIELD
    private int _numberObjectiveHolding;
    #endregion

    public static event Action<int> setCharacterObjectiveHold;

    private void Awake()
    {
        Objective.pickObjectiveEvent += OnObjectivePicked;
    }

    private void OnDestroy()
    {
        Objective.pickObjectiveEvent -= OnObjectivePicked;
    }

    private void OnObjectivePicked()
    {
        _numberObjectiveHolding++;

        setCharacterObjectiveHold?.Invoke(_numberObjectiveHolding);
    }
}
