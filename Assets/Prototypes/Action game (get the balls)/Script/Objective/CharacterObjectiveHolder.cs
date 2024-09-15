using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterObjectiveHolder : MonoBehaviour
{
    #region PRIVATE FIELD
    private int _numberObjectiveHolding;
    #endregion

    #region PRIVATE FIELD
    public static event Action<int> setCharacterObjectiveHold;
    public static event Action<int> earnObjectiveEvent;
    #endregion

    private void Awake()
    {
        Objective.pickObjectiveEvent += OnObjectivePicked;
        DeliveryTargetLocation.objectiveDeliveredEvent += OnObjectiveDelivered;
    }

    private void OnDestroy()
    {
        Objective.pickObjectiveEvent -= OnObjectivePicked;
        DeliveryTargetLocation.objectiveDeliveredEvent -= OnObjectiveDelivered;
    }

    private void OnObjectivePicked()
    {
        _numberObjectiveHolding++;

        setCharacterObjectiveHold?.Invoke(_numberObjectiveHolding);
    }

    private void OnObjectiveDelivered()
    {
        if (_numberObjectiveHolding > 0)
        {

        }
        else
        {

        }

        setCharacterObjectiveHold?.Invoke(0);

        earnObjectiveEvent?.Invoke(_numberObjectiveHolding);
    }
}
