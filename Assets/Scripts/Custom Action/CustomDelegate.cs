using ReedJim.RPG.Stat;
using UnityEngine;
using UnityEngine.VFX;

public class CustomDelegate
{
    public delegate bool BoolAction();
    public delegate Rigidbody GetRigidbodyAction();
    public delegate CharacterStat GetCharacterStatAction<in T>(T arg);
    public delegate IProjectile GetIProjectileAction();
    public delegate Bullet GetBulletAction();
    public delegate ExplosiveBullet GetExplosiveBulletAction();
    public delegate ExplosiveAreaIndicator GetExplosiveAreaIndicatorAction();
    public delegate ICollectible GetICollectibleAction();
    public delegate VisualEffect GetVisualEffectAction();
}
