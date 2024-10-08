using System;
using UnityEngine;
using static CustomDelegate;

public class CharacterRangedAttack : MonoBehaviour, ICharacterAttack
{
    [SerializeField] private string expectedProjectileTag;
    [SerializeField] private string expectedProjectileLayer;
    [SerializeField] private Transform shootingSphere;

    private bool _isShootingSphereEffectInProgress;

    public static event GetIProjectileAction getIProjectileEvent;

    private void Awake()
    {
        BaseCharacterVision.attackEnemyEvent += RangedAttack;
    }

    private void OnDestroy()
    {
        BaseCharacterVision.attackEnemyEvent -= RangedAttack;
    }

    public void Attack()
    {
        // Not neccesary to use interface here, cuz event already used
        // RangedAttack();
    }

    private void RangedAttack(int instanceId, Transform target)
    {
        if (instanceId == gameObject.GetInstanceID())
        {
            IProjectile bullet = GetProjectile();

            bullet.GameObject.tag = expectedProjectileTag;
            bullet.GameObject.layer = LayerMask.NameToLayer(expectedProjectileLayer); ;

            Vector3 shotPosition = shootingSphere.position;

            // // only shoot if target is in front of shooting sphere
            // if ((target.position.x - shootingSphere.position.x) / (transform.position.x - shootingSphere.position.x) > 0)
            // {
            //     return;
            // }

            if (!_isShootingSphereEffectInProgress)
            {
                StartCoroutine(SpringAnimation.SpringScaleAnimation(shootingSphere, 0.2f * Vector3.one, 0.1f, 4, 0.1f, onCompletedAction: () =>
                {
                    _isShootingSphereEffectInProgress = false;
                }));

                _isShootingSphereEffectInProgress = true;
            }


            bullet.Shoot(target, shotPosition, attackInstanceId: gameObject.GetInstanceID());

            // bullet.gameObject.SetActive(true);

            // setBulletAttackerInstanceId?.Invoke(bullet.gameObject.GetInstanceID(), gameObject.GetInstanceID());

            // Vector3 shotPosition = transform.position + new Vector3(0, 2, 0);

            // bullet.transform.position = shotPosition;

            // ShootBullet(bullet, target, shotPosition);
        }
    }

    protected virtual IProjectile GetProjectile()
    {
        return getIProjectileEvent?.Invoke();
    }

    // protected virtual void ShootBullet(Rigidbody bullet, Transform target, Vector3 shotPosition)
    // {
    //     Vector3 direction = target.position - shotPosition;

    //     direction.y = directionYMultiplier;
    //     Debug.Log(forceMultiplier * direction);

    //     bullet.AddForce(forceMultiplier * direction, ForceMode.Impulse);
    // }
}
