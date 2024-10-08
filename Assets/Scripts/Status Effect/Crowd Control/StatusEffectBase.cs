using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectBase : ScriptableObject, IStatusEffect
{
    [SerializeField] private float duration;

    public virtual void ApplyStatusEffect(int instanceId)
    {

    }
}
