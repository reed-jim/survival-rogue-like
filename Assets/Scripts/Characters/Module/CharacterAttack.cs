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
    public static event Action<CharacterState> setCharacterState;
    public static event Action<string, int> setCharacterAnimationIntProperty;
    public static event Action<string, float> setCharacterAnimationFloatProperty;
    #endregion

    #region LIFE CYCLE
    private void Awake()
    {
        _tweens = new List<Tween>();

        meleeAttackCollider.enabled = false;
    }


    private void OnDestroy()
    {

    }
    #endregion

    public void MeleeAttack()
    {
        setCharacterAnimationIntProperty?.Invoke("Speed", 0);
        setCharacterAnimationFloatProperty?.Invoke("State", 1);

        _tweens.Add(Tween.Delay(1.3f).OnComplete(() => meleeAttackCollider.gameObject.SetActive(true)));
        _tweens.Add(Tween.Delay(5f).OnComplete(() =>
        {
            setCharacterAnimationFloatProperty?.Invoke("State", 1);

            setCharacterState?.Invoke(CharacterState.IDLE);
        }));

        setCharacterState?.Invoke(CharacterState.ATTACK);
    }
}
