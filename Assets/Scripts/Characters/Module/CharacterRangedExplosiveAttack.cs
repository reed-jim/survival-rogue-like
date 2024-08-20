using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CustomDelegate;

public class CharacterRangedExplosiveAttack : CharacterRangedAttack
{
    public static event GetExplosiveBulletAction getExplosiveBulletEvent;

    protected override IProjectile GetProjectile()
    {
        return getExplosiveBulletEvent?.Invoke();
    }

    protected override void ShootBullet(Rigidbody bullet, Transform target, Vector3 shotPosition)
    {
        base.ShootBullet(bullet, target, shotPosition);
    }
}
