using System;
using UnityEngine;
using static CustomDelegate;

public class CharacterRangedAttack : MonoBehaviour, ICharacterAttack
{
    [Header("CUSTOMIZE")]
    [SerializeField] private float forceMultiplier;

    public static event GetBulletAction getBulletEvent;

    private void Awake()
    {
        CharacterVision.attackEnemyEvent += RangedAttack;
    }

    private void OnDestroy()
    {
        CharacterVision.attackEnemyEvent -= RangedAttack;
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

            Vector3 shotPosition = transform.position + new Vector3(0, 2, 0);

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
        return getBulletEvent?.Invoke();
    }

    protected virtual void ShootBullet(Rigidbody bullet, Transform target, Vector3 shotPosition)
    {
        bullet.AddForce(forceMultiplier * (target.position - shotPosition));
    }
}
