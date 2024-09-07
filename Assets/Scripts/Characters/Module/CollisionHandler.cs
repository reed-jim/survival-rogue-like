using System;
using System.Linq;
using ReedJim.RPG.Stat;
using UnityEngine;
using UnityEngine.VFX;
using static CustomDelegate;

public class CollisionHandler : MonoBehaviour
{
    private IExplodable _explodableObject;
    private IHitByMeleeAttack _hitByMeleeAttackObject;

    [Header("CUSTOMIZE")]
    [SerializeField] private string[] collideTags;

    #region ACTION
    public static event Action<int, CharacterStat> applyDamageEvent;
    public static event GetCharacterStatAction<int> getAttackerStatAction;
    public static event Action<int> characterHitEvent;
    public static GetVisualEffectAction getVisualEffectEvent;
    #endregion

    private void Awake()
    {
        _explodableObject = GetComponent<IExplodable>();
        _hitByMeleeAttackObject = GetComponent<IHitByMeleeAttack>();
    }

    private void OnCollisionEnter(Collision other)
    {
        GameObject otherGameObject = other.collider.gameObject;
        
        if (collideTags.Contains(otherGameObject.tag))
        {
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

        // HandleOnCollisionExplosive(otherGameObject);
        // HandleOnBeingMeleeAttacked(otherGameObject);
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
