using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Dreamteck.Splines;
using PrimeTween;
using Saferio.Util.SaferioTween;
using Unity.Netcode;

#if UNITY_EDITOR
using UnityEditor.Animations;
#endif

using UnityEngine;

public class PlayerAttack : NetworkBehaviour
{
    [Header("WEAPON")]
    private IWeapon weapon;

    [Header("FX")]
    [SerializeField] private ParticleSystem attackFx;

    #region PRIVATE FIELD
    private Animator _animator;
    private bool _isAttacking;
    private bool _isEnableInput = true;
    #endregion

    #region PROPERTY
    public IWeapon Weapon
    {
        get => weapon; set => weapon = value;
    }
    #endregion

    #region ACTION
    public static event Action<int, bool> enableRotatingEvent;
    public static event Action<int> playAttackSFXEvent;
    #endregion

    #region LIFE CYCLE
    private void Awake()
    {
        _animator = GetComponent<Animator>();

        LevelingUI.enableInput += EnableInput;
        VikingWhirlwing.playAttackFxEvent += PlayAttackFx;

        // _actualAttackAnimationDuration = GetActualAttackAnimationDuration();

        StartCoroutine(AutoMeleeAttack());
    }

    public override void OnDestroy()
    {
        LevelingUI.enableInput -= EnableInput;
        VikingWhirlwing.playAttackFxEvent -= PlayAttackFx;

        // _waitForEndAttackAnimationTween.Stop();
    }
    #endregion

    private void EnableInput(bool isEnable)
    {
        _isEnableInput = isEnable;
    }

    private IEnumerator AutoMeleeAttack()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(2f);

        yield return waitForSeconds;

        while (true)
        {
            yield return new WaitUntil(() => IsSpawned);

            // if (IsServer) yield break;

            if (IsOwner)
            {
                MeleeAttack();
            }

            // MeleeAttackRpc();

            yield return waitForSeconds;
        }
    }

    [Rpc(SendTo.NotOwner)]
    private void MeleeAttackRpc()
    {
        if (!IsOwner)
        {
            MeleeAttack();
        }
    }

    private void MeleeAttack()
    {
        if (_isAttacking)
        {
            return;
        }
        else
        {
            _isAttacking = true;
        }

        _animator.SetInteger("State", 1);

        playAttackSFXEvent?.Invoke(gameObject.GetInstanceID());

        enableRotatingEvent?.Invoke(gameObject.GetInstanceID(), false);

        SaferioTween.Delay(0.5f, onCompletedAction: () =>
        {
            _animator.SetInteger("State", 0);

            enableRotatingEvent?.Invoke(gameObject.GetInstanceID(), true);

            _isAttacking = false;
        });

        weapon.Attack();
    }

    private void PlayAttackFx(bool isPlay)
    {
        if (isPlay)
        {
            attackFx.Play();
        }
        else
        {
            attackFx.Stop();
        }
    }

    private float GetActualAttackAnimationDuration()
    {
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

        return attackState.motion.averageDuration / attackState.speed;
#else
        return 1;
#endif
    }

    // private IEnumerator Shooting()
    // {
    //     while (true)
    //     {
    //         if (_playerStat == null)
    //         {
    //             _playerStat = (PlayerStat)(getStatEvent?.Invoke());

    //             continue;
    //         }

    //         Collider[] hitColliders = Physics.OverlapSphere(transform.position, _playerStat.AttackRange, layerMaskCheckRangedAttack);

    //         foreach (var hitCollider in hitColliders)
    //         {
    //             if (hitCollider.transform.tag == Constants.ENEMY_TAG)
    //             {
    //                 Enemy enemy = hitCollider.transform.parent.GetComponent<Enemy>();

    //                 if (enemy == null)
    //                 {
    //                     continue;
    //                 }

    //                 if (enemy.State == CharacterState.DIE)
    //                 {
    //                     continue;
    //                 }

    //                 Vector3 hitPoint = Physics.ClosestPoint(transform.position, hitCollider, hitCollider.transform.position, hitCollider.transform.rotation);

    //                 playerShooterController.Shoot(hitCollider.transform.parent, transform);

    //                 break;
    //             }
    //         }

    //         yield return new WaitForSeconds(playerStatObserver.PlayerStat.ReloadTime);
    //     }
    // }

    // private void ManualShoot()
    // {
    //     Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);

    //     playerShooterController.Shoot(hit.point, transform);
    // }

    // private void PlaySwordTrail()
    // {
    //     swordTrail?.Play();

    //     // Tween.Delay(0.2f * _actualAttackAnimationDuration).OnComplete(() =>
    //     // {
    //     //     swordTrail?.Stop();
    //     // });

    //     // Tween.Delay(0.1f).OnComplete(() =>
    //     // {
    //     //     swordTrail?.Stop();
    //     // });

    //     StartCoroutine(MoveAlongSpline());

    //     IEnumerator MoveAlongSpline()
    //     {
    //         WaitForSeconds waitForSeconds = new WaitForSeconds(3f * Time.deltaTime);

    //         bool isTrailStop = false;
    //         float progress = 0;

    //         float deltaProgress = Time.deltaTime / 0.2f;

    //         while (progress < 1)
    //         {
    //             swordSlashSplineFollower.Move((double)deltaProgress);

    //             progress += deltaProgress;

    //             if (progress > 0.4f && !isTrailStop)
    //             {
    //                 swordTrail?.Stop();

    //                 isTrailStop = true;
    //             }

    //             yield return waitForSeconds;
    //         }
    //     }

    //     // Tween.Custom(0, 1, duration: 0.8f, onValueChange: newVal => swordSlashSplineFollower.Move((double)0.05))
    //     // .OnComplete(() =>
    //     // {
    //     //     swordTrail?.Stop();
    //     // });
    // }

    // private void PlaySwordSlash()
    // {
    //     swordSlashTemp.gameObject.SetActive(true);

    //     Tween.Delay(0.6f * _actualAttackAnimationDuration).OnComplete(() =>
    //     {
    //         Tween.Custom(0, 1, duration: 0.9f * _actualAttackAnimationDuration, onValueChange: newVal => sworldSlashMaterial.SetFloat("_Slash", newVal));
    //     });

    //     // Tween.Custom(0, 1, duration: _actualAttackAnimationDuration, onValueChange: newVal => sworldSlashMaterial.SetFloat("_Slash", newVal));

    //     Sequence.Create()
    //         .Chain(Tween.Custom(0, -90, duration: 0.1f, onValueChange: newVal =>
    //         {
    //             hammer.transform.localEulerAngles = new Vector3(0, newVal, 0);
    //         }))
    //         .Chain(Tween.Custom(-90, 90, duration: 0.25f, onValueChange: newVal =>
    //         {
    //             hammer.transform.localEulerAngles = new Vector3(0, newVal, 0);
    //         }))
    //         .Chain(Tween.Custom(90, 0, duration: 0.55f, onValueChange: newVal =>
    //         {
    //             hammer.transform.localEulerAngles = new Vector3(0, newVal, 0);
    //         }));

    //     // Tween.Custom(0, 70, duration: 0.4f, cycles: 1, cycleMode: CycleMode.Yoyo, onValueChange: newVal =>
    //     // {
    //     //     hammer.transform.localEulerAngles = new Vector3(0, newVal, 0);
    //     // })
    //     // .OnComplete(() =>
    //     // {
    //     //     Tween.Custom(0, 70, duration: 0.5f, onValueChange: newVal =>
    //     //     {
    //     //         hammer.transform.eulerAngles = _prevHammerEulerAngle + new Vector3(0, newVal, 0);

    //     //         _prevHammerEulerAngle = hammer.transform.eulerAngles;
    //     //     });
    //     // });

    //     // swordSlash.Play();
    // }
}
