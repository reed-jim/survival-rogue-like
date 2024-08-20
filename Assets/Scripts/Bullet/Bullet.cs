using System;
using PrimeTween;
using UnityEngine;

public class Bullet : MonoBehaviour, IProjectile, IContainParentInstanceId
{
    [Header("CUSTOMIZE")]
    [SerializeField] protected float forceMultiplier;
    [SerializeField] private float maxExistingTime;

    [Header("MANAGEMENT")]
    [SerializeField] protected TrailRenderer bulletTrail;
    protected Rigidbody _rigidBody;
    protected int _attackerInstanceId;

    protected virtual void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();

        // CharacterRangedAttack.setBulletAttackerInstanceId += SetAttackInstanceId;
    }

    protected virtual void OnEnable()
    {
        Tween.Delay(maxExistingTime).OnComplete(() => gameObject.SetActive(false));
    }

    private void OnDisable()
    {
        bulletTrail.Clear();
    }

    private void OnDestroy()
    {
        // CharacterRangedAttack.setBulletAttackerInstanceId -= SetAttackInstanceId;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        gameObject.SetActive(false);

        // if (collision.gameObject.tag != Constants.PLAYER_TAG)
        // {
        //     gameObject.SetActive(false);
        // }
    }

    public virtual void Shoot(Transform target, Vector3 shotPosition, int attackInstanceId)
    {
        gameObject.SetActive(true);

        SetAttackInstanceId(attackInstanceId);

        transform.position = shotPosition;

        _rigidBody.AddForce(forceMultiplier * (target.position - shotPosition));
    }

    public int GetParentInstanceId()
    {
        return _attackerInstanceId;
    }

    public void SetAttackInstanceId(int attackInstanceId)
    {
        _attackerInstanceId = attackInstanceId;
    }
}
