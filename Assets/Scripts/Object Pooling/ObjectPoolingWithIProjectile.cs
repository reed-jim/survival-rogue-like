using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingWithIProjectile : ObjectPoolingWithOneComponent<IProjectile>
{
    protected override void Awake()
    {
        base.Awake();

        CharacterRangedAttack.getIProjectileEvent += GetComponentFromPool;
    }

    private void OnDestroy()
    {
        CharacterRangedAttack.getIProjectileEvent -= GetComponentFromPool;
    }
}
