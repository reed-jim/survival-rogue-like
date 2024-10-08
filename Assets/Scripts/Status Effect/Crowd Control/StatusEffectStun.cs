using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Status Effect - Stun", menuName = "ScriptableObjects/RPG/StatusEffectStun")]
public class StatusEffectStun : StatusEffectBase
{
    public static event Action<int> applyStunEffectEvent;

    public override void ApplyStatusEffect(int instanceId)
    {
        applyStunEffectEvent?.Invoke(instanceId);
    }
}
