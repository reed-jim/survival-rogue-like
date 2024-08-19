using PrimeTween;
using UnityEngine;

public class Bullet : MonoBehaviour, IProjectile, IContainParentInstanceId
{
    [Header("CUSTOMIZE")]
    [SerializeField] private float forceMultiplier;
    [SerializeField] private float maxExistingTime;

    [Header("MANAGEMENT")]
    [SerializeField] protected TrailRenderer bulletTrail;
    protected Rigidbody _rigidBody;
    protected int _attackerInstanceId;

    protected virtual void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();

        CharacterRangedAttack.setBulletAttackerInstanceId += SetAttackInstanceId;
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
        CharacterRangedAttack.setBulletAttackerInstanceId -= SetAttackInstanceId;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        gameObject.SetActive(false);

        // if (collision.gameObject.tag != Constants.PLAYER_TAG)
        // {
        //     gameObject.SetActive(false);
        // }
    }

    public void Shoot(Vector3 direction)
    {
        _rigidBody.AddForce(forceMultiplier * direction);
    }

    public int GetParentInstanceId()
    {
        return _attackerInstanceId;
    }

    public void SetAttackInstanceId(int bulletInstanceId, int attackInstanceId)
    {
        if (bulletInstanceId == gameObject.GetInstanceID())
        {
            _attackerInstanceId = attackInstanceId;
        }
    }
}
