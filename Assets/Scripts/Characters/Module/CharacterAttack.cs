
using System;
using System.Collections.Generic;
using PrimeTween;
#if UNITY_EDITOR
using UnityEditor.Animations;
#endif
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    [Header("COLLIDER")]
    [SerializeField] private Collider meleeAttackCollider;

    [Header("FX")]
    [SerializeField] private ParticleSystem weaponTrailFx;

    [Header("CUSTOMIZE")]
    [SerializeField] private float delayTimeAttackHit;

    #region PRIVATE FIELD
    private List<Tween> _tweens;
    private Animator _animator;
    private CharacterStatManager _characterStatManager;
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

        _animator = GetComponent<Animator>();
        _characterStatManager = GetComponent<CharacterStatManager>();

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

        setCharacterAnimationIntProperty?.Invoke(gameObject.GetInstanceID(), "State", Constants.ANIMATION_ATTACK_STATE);
        setCharacterAnimationFloatProperty?.Invoke(gameObject.GetInstanceID(), "Speed", 0);

        weaponTrailFx.Play();

        _tweens.Add(Tween.Delay(delayTimeAttackHit).OnComplete(() =>
        {
            meleeAttackCollider.gameObject.SetActive(true);

            // _tweens.Add(Tween.Delay(0.02f).OnComplete(() =>
            // {
            //     meleeAttackCollider.gameObject.SetActive(false);
            // }));
        }));

        Tween.Delay(0.5f).OnComplete(() =>
        {
            _tweens.Add(Tween.Delay(GetActualAttackAnimationDuration() - 0.5f).OnComplete(() =>
            {
                setCharacterAnimationIntProperty?.Invoke(gameObject.GetInstanceID(), "State", Constants.ANIMATION_MOVEMENT_STATE);

                meleeAttackCollider.gameObject.SetActive(false);
            }));

            weaponTrailFx.Stop();
        });

        Debug.Log(_characterStatManager);
        Debug.Log(_characterStatManager.Stat);

        if (_characterStatManager.Stat == null)
        {
            return;
        }

        _tweens.Add(Tween.Delay(_characterStatManager.Stat.GetStatValue(StatComponentNameConstant.AttackSpeed)).OnComplete(() =>
        {
            setCharacterState?.Invoke(gameObject.GetInstanceID(), CharacterState.IDLE);
        }));

        setCharacterState?.Invoke(gameObject.GetInstanceID(), CharacterState.ATTACK);
    }

    private float GetActualAttackAnimationDuration()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        float duration = stateInfo.length;

        return duration;

        // if (stateInfo.IsName(clip.name))
        // {
        //     float duration = clip.length;
        //     Debug.Log("Current animation duration: " + duration);

        //     return duration;
        // }

#if UNITY_EDITOR
        AnimatorController runtimeAnimatorController = _animator.runtimeAnimatorController as AnimatorController;

        AnimatorControllerLayer[] acLayers = runtimeAnimatorController.layers;

        AnimatorState attackState = new AnimatorState();

        foreach (AnimatorControllerLayer i in acLayers)
        {
            ChildAnimatorState[] animStates = i.stateMachine.states;

            foreach (ChildAnimatorState j in animStates)
            {
                if (j.state.name == "Sword And Shield Slash")
                {
                    attackState = j.state;
                }
            }
        }

        Debug.Log(attackState.motion.averageDuration / attackState.speed);

        return attackState.motion.averageDuration / attackState.speed;
#else
        return 1;
#endif
    }
}
