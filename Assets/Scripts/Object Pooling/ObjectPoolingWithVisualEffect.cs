using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ObjectPoolingWithVisualEffect : ObjectPoolingWithOneComponent<VisualEffect>
{
    protected override void Awake()
    {
        base.Awake();

        CollisionHandler.getVisualEffectEvent += GetComponentFromPool;
    }

    private void OnDestroy()
    {
        CollisionHandler.getVisualEffectEvent -= GetComponentFromPool;
    }
}
