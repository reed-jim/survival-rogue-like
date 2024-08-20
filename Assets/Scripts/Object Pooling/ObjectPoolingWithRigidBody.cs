using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingWithRigidBody : ObjectPoolingWithOneComponent<Rigidbody>
{
    protected override void Awake()
    {
        base.Awake();

        // CharacterRangedAttack.getRigidbodyEvent += GetComponentFromPool;
    }

    private void OnDestroy()
    {
        // CharacterRangedAttack.getRigidbodyEvent -= GetComponentFromPool;
    }
}
