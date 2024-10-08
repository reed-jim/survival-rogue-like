using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Status Effect - Rooted", menuName = "ScriptableObjects/RPG/StatusEffectRooted")]
public class StatusEffectRooted : StatusEffectBase
{
    public static event Action<int> applyRootedEffectEvent;

    public override void ApplyStatusEffect(int instanceId)
    {
        applyRootedEffectEvent?.Invoke(instanceId);
    }
}
