using System;
using PrimeTween;
using ReedJim.RPG.Stat;
using UnityEngine;
using static CustomDelegate;

public class Bullet : MonoBehaviour, IProjectile, IContainParentInstanceId, ICollide
{
    [Header("CUSTOMIZE")]
    [SerializeField] protected float forceMultiplier;
    [SerializeField] private float maxExistingTime;

    #region PRIVATE FIELD
    [SerializeField] protected TrailRenderer bulletTrail;
    protected Rigidbody _rigidBody;
    protected int _attackerInstanceId;
    #endregion

    #region ACTION
    public static event Action<int, CharacterStat> applyDamageEvent;
    public static event Action<int> characterHitEvent;
    public static event GetCharacterStatAction<int> getAttackerStatAction;
    #endregion

    public GameObject GameObject
    {
        get => gameObject;
    }

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
        // gameObject.SetActive(false);

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

        // REMEMBER TO RESET THE VELOCITY BEFORE APPLY FORCE
        _rigidBody.velocity = Vector3.zero;

        Vector3 direction = (target.position - shotPosition).normalized;

        direction.y = 0;

        transform.rotation = Quaternion.LookRotation(direction);

        _rigidBody.AddForce(forceMultiplier * direction, ForceMode.Impulse);

        // _rigidBody.AddForce(forceMultiplier * (AvoidTooNearTarget(target.position, shotPosition) - shotPosition).normalized, ForceMode.Impulse);
    }

    private Vector3 AvoidTooNearTarget(Vector3 targetPosition, Vector3 shotPosition)
    {
        if (Vector3.Distance(targetPosition, shotPosition) < 2)
        {
            return targetPosition *= 5;
        }
        else
        {
            return targetPosition;
        }
    }

    public int GetParentInstanceId()
    {
        return _attackerInstanceId;
    }

    public void SetAttackInstanceId(int attackInstanceId)
    {
        _attackerInstanceId = attackInstanceId;
    }

    protected CharacterStat GetAttackerStat()
    {
        return getAttackerStatAction.Invoke(_attackerInstanceId);
    }

    #region ICollide Implement
    public void HandleOnCollide(GameObject other)
    {
        applyDamageEvent?.Invoke(other.GetInstanceID(), GetAttackerStat());

        characterHitEvent?.Invoke(other.GetInstanceID());
    }
    #endregion
}
