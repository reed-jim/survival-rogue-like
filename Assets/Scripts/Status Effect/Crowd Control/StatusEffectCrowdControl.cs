using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectCrowdControl : IStatusEffect
{
    public static event Action<int> applyStunEffectEvent;

    public void ApplyStatusEffect(int instanceId)
    {
        applyStunEffectEvent?.Invoke(instanceId);
    }
}
