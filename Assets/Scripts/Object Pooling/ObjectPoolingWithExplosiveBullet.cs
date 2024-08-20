using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingWithExplosiveBullet : ObjectPoolingWithOneComponent<ExplosiveBullet>
{
    protected override void Awake()
    {
        base.Awake();

        CharacterRangedExplosiveAttack.getExplosiveBulletEvent += GetComponentFromPool;
    }

    private void OnDestroy()
    {
        CharacterRangedExplosiveAttack.getExplosiveBulletEvent -= GetComponentFromPool;
    }
}
