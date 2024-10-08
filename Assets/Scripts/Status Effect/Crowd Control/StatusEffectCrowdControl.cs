using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/RPG/StatusEffectCrowdControl")]
public class StatusEffectCrowdControl : ScriptableObject
{
    [SerializeField] protected float duration;

    // public static event Action<int> applyStunEffectEvent;

    // public void ApplyStatusEffect(int instanceId)
    // {
    //     applyStunEffectEvent?.Invoke(instanceId);
    // }
}
