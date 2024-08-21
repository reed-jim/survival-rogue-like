using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingWithExplosiveAreaIndicator : ObjectPoolingWithOneComponent<ExplosiveAreaIndicator>
{
    protected override void Awake()
    {
        base.Awake();

        ExplosiveBullet.getExplosiveAreaIndicatorAction += GetComponentFromPool;
    }

    private void OnDestroy()
    {
        ExplosiveBullet.getExplosiveAreaIndicatorAction -= GetComponentFromPool;
    }
}
