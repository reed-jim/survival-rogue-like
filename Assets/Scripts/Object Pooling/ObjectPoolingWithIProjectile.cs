using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingWithIProjectile : ObjectPoolingWithOneComponent<IProjectile>
{
    protected override void Awake()
    {
        base.Awake();
    }

    private void OnDestroy()
    {

    }
}
