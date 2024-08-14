using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingWithRigidBody : ObjectPoolingWithOneComponent<Rigidbody>
{
    protected override void Awake()
    {
        base.Awake();

        RangedAttack.getRigidbodyEvent += GetComponentFromPool;
    }

    private void OnDestroy()
    {
        RangedAttack.getRigidbodyEvent -= GetComponentFromPool;
    }
}
