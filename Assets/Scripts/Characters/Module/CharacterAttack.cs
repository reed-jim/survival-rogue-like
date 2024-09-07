using System;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    [Header("COLLIDER")]
    [SerializeField] private Collider meleeAttackCollider;

    #region PRIVATE FIELD
    private List<Tween> _tweens;
    #endregion

    #region ACTION
    public static event Action<int, CharacterState> setCharacterState;
    public static event Action<int, string, int> setCharacterAnimationIntProperty;
    public static event Action<int, string, float> setCharacterAnimationFloatProperty;
    #endregion

    #region LIFE CYCLE
    private void Awake()
    {
        _tweens = new List<Tween>();

        BaseCharacterVision.attackEnemyEvent += MeleeAttack;

        meleeAttackCollider.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        BaseCharacterVision.attackEnemyEvent -= MeleeAttack;
    }
    #endregion

    public void MeleeAttack(int instanceId, Transform enemy)
    {
        if (instanceId != gameObject.GetInstanceID())
        {
            return;
        }

        transform.LookAt(enemy);

        setCharacterAnimationIntProperty?.Invoke(gameObject.GetInstanceID(), "State", 1);
        setCharacterAnimationFloatProperty?.Invoke(gameObject.GetInstanceID(), "Speed", 0);

        _tweens.Add(Tween.Delay(1.3f).OnComplete(() =>
        {
            meleeAttackCollider.gameObject.SetActive(true);

            _tweens.Add(Tween.Delay(0.02f).OnComplete(() =>
            {
                meleeAttackCollider.gameObject.SetActive(false);

            }));
        }));
        _tweens.Add(Tween.Delay(5f).OnComplete(() =>
        {
            setCharacterAnimationIntProperty?.Invoke(gameObject.GetInstanceID(), "State", 0);

            setCharacterState?.Invoke(gameObject.GetInstanceID(), CharacterState.IDLE);
        }));

        setCharacterState?.Invoke(gameObject.GetInstanceID(), CharacterState.ATTACK);
    }
}
