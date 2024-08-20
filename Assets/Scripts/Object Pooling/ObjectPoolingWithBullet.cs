using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingWithBullet : ObjectPoolingWithOneComponent<Bullet>
{
    protected override void Awake()
    {
        base.Awake();

        CharacterRangedAttack.getBulletEvent += GetComponentFromPool;
    }

    private void OnDestroy()
    {
        CharacterRangedAttack.getBulletEvent -= GetComponentFromPool;
    }
}
