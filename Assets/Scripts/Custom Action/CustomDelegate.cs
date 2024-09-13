using ReedJim.RPG.Stat;
using UnityEngine;
using UnityEngine.VFX;

public class CustomDelegate
{
    public delegate bool BoolAction();
    public delegate Vector3 GetVector3Action();
    public delegate GameObject GetGameObjectAction();
    public delegate Transform GetTransformAction();
    public delegate Rigidbody GetRigidbodyAction();
    public delegate CharacterStat GetCharacterStatAction<in T>(T arg);
    public delegate IProjectile GetIProjectileAction();
    public delegate Bullet GetBulletAction();
    public delegate ExplosiveBullet GetExplosiveBulletAction();
    public delegate ExplosiveAreaIndicator GetExplosiveAreaIndicatorAction();
    public delegate ICollectible GetICollectibleAction();
    public delegate VisualEffect GetVisualEffectAction();

    #region ACTIVE SKILL
    public delegate Meteor GetMeteorAction();
    #endregion
}
