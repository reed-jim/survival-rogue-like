using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryTargetLocation : MonoBehaviour
{
    public static event Action objectiveDeliveredEvent;
    
    private void OnTriggerEnter(Collider other)
    {
        objectiveDeliveredEvent?.Invoke();
    }
}
