using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class BaseCharacterVision : MonoBehaviour
{
    public static event Action<int, Transform> attackEnemyEvent;

    protected void InvokeAttackEnemyEvent(int instanceId, Transform enemy)
    {
        attackEnemyEvent?.Invoke(instanceId, enemy);
    }
}
