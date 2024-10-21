using System;
using System.Linq;
using ReedJim.RPG.Stat;
using Saferio.Util.SaferioTween;
using UnityEngine;
using UnityEngine.VFX;
using static CustomDelegate;

public class CollisionHandler : MonoBehaviour
{
    private IExplodable _explodableObject;
    private IHitByMeleeAttack _hitByMeleeAttackObject;

    [Header("CUSTOMIZE")]
    [SerializeField] private string[] collideTags;

    #region PRIVATE FIELD
    private bool _isJustCollided;
    private bool _isCharacterDied;
    #endregion

    #region ACTION
    public static event Action<int, CharacterStat> applyDamageEvent;
    public static event GetCharacterStatAction<int> getAttackerStatAction;
    public static event Action<int> characterHitEvent;
    public static GetVisualEffectAction getVisualEffectEvent;
    public static event Action<int> disableNavMeshEvent;
    #endregion

    private void Awake()
    {
        CharacterStatManager.characterDieEvent += SetIsCharacterDie;

        _explodableObject = GetComponent<IExplodable>();
        _hitByMeleeAttackObject = GetComponent<IHitByMeleeAttack>();

        _isCharacterDied = false;
    }

    private void OnDestroy()
    {
        CharacterStatManager.characterDieEvent -= SetIsCharacterDie;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isCharacterDied)
        {
            return;
        }

        if (_isJustCollided)
        {
            return;
        }
        else
        {
            SaferioTween.Delay(0.5f, onCompletedAction: () => _isJustCollided = false);

            _isJustCollided = true;
        }

        GameObject otherGameObject = other.gameObject;

        if (collideTags.Contains(otherGameObject.tag))
        {
            disableNavMeshEvent?.Invoke(GetInstanceID());

            Rigidbody rbA = GetComponent<Rigidbody>();

            rbA.velocity = Vector3.zero;

            Vector3 force = 75 * (transform.position - otherGameObject.transform.position);

            force.y = 0;

            rbA.AddForce(force, ForceMode.Impulse);








            ICollide collidable = otherGameObject.GetComponent<ICollide>();

            if (collidable == null)
            {
                collidable = otherGameObject.transform.parent.GetComponent<ICollide>();
            }

            if (collidable != null)
            {
                collidable.HandleOnCollide(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        return;

        GameObject otherGameObject = other.collider.gameObject;






        Rigidbody rbA = GetComponent<Rigidbody>();
        Rigidbody rbB = other.rigidbody;

        if (rbB != null)
        {
            // Calculate the collision force
            Vector3 collisionForce = other.relativeVelocity * rbB.mass;

            // Check if the force exceeds the limit
            if (collisionForce.magnitude > 1)
            {
                // Calculate the force to apply
                Vector3 forceToApply = collisionForce.normalized * 1;

                // Apply the limited force to the other rigidbody
                // rbA.AddForce(-forceToApply, ForceMode.Impulse);
            }
        }



        if (collideTags.Contains(otherGameObject.tag))
        {
            rbA.velocity = Vector3.zero;

            Vector3 force = 20 * (transform.position - otherGameObject.transform.position);

            force.y = 50;

            rbA.AddForce(force, ForceMode.Impulse);




            ICollide collidable = otherGameObject.GetComponent<ICollide>();

            if (collidable == null)
            {
                collidable = otherGameObject.transform.parent.GetComponent<ICollide>();
            }

            if (collidable != null)
            {
                collidable.HandleOnCollide(gameObject);
            }
        }
    }

    private void SetIsCharacterDie(int instanceId)
    {
        if (instanceId == gameObject.GetInstanceID())
        {
            _isCharacterDied = true;
        }
    }






    private void HandleOnCollisionExplosive(GameObject otherGameObject)
    {
        if (CommonUtil.IsNull(_explodableObject))
        {
            return;
        }

        // // solution 1
        // if (other.gameObject.tag == Constant.TAG_EXPLOSIVE)
        // {
        //     _explodableObject.Explode();
        // }

        // solution 2
        IExplosive explosive = otherGameObject.GetComponent<IExplosive>();

        if (CommonUtil.IsNotNull(explosive))
        {
            _explodableObject.Explode();
        }
    }

    private void HandleOnBeingMeleeAttacked(GameObject weapon)
    {
        if (collideTags.Contains(weapon.tag))
        {
            float damage = 0;

            GameObject attacker = CommonUtil.GetParentGameObject(weapon);

            CharacterStat characterStat = getAttackerStatAction.Invoke(attacker.GetInstanceID());

            if (characterStat != null)
            {
                damage = characterStat.GetStatValue(StatComponentNameConstant.Damage);
            }

            applyDamageEvent?.Invoke(gameObject.GetInstanceID(), characterStat);

            characterHitEvent?.Invoke(gameObject.GetInstanceID());

            VisualEffect visualEffect = getVisualEffectEvent?.Invoke();

            visualEffect.gameObject.SetActive(true);
            visualEffect.transform.position = transform.position + 0.0f * transform.forward;
            visualEffect.Play();
        }

        // if (CommonUtil.IsNull(_hitByMeleeAttackObject))
        // {
        //     return;
        // }

        // _hitByMeleeAttackObject.OnHit();

        // // solution 1
        // MeleeAttack meleeAttack = otherGameObject.GetComponent<MeleeAttack>();

        // meleeAttack.Attack(gameObject.GetInstanceID());

        // // solution 2
        // // use Action
    }

    private void HandleOnBeingHitByBullet(GameObject weapon)
    {
        if (collideTags.Contains(weapon.tag))
        {
            float damage = 0;

            CharacterStat characterStat = getAttackerStatAction.Invoke(weapon.GetComponent<Bullet>().GetParentInstanceId());

            if (characterStat != null)
            {
                damage = characterStat.GetStatValue(StatComponentNameConstant.Damage);
            }

            applyDamageEvent?.Invoke(gameObject.GetInstanceID(), characterStat);
        }
    }
}
