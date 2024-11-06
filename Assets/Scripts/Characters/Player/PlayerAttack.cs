using System;
using System.Collections;
using System.Collections.Generic;
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
    [Header("FAKE WHIRL WIND")]
    [SerializeField] private Rigidbody fakeWhirlwindAttackRigidBody;

    [Header("ANIMATION")]
    private float _actualAttackAnimationDuration;

    // [Header("TEMP")]
    // [SerializeField] private Transform hammer;
    // [SerializeField] private Material sworldSlashMaterial;
    // [SerializeField] private GameObject swordSlashTemp;

    [Header("COLLIDER")]
    [SerializeField] private Collider swordCollider;

    [Header("FX")]
    [SerializeField] private ParticleSystem attackFx;

    [Header("CUSTOMIZE")]
    [SerializeField] private float delayTimeAttackHit;

    #region PRIVATE FIELD
    private Animator _animator;
    private bool _isAttacking;
    private bool _isEnableInput = true;



    private bool _isTest;
    private Vector3 _lastAngle;
    private Vector3 _initialAttackFxAngle;
    #endregion

    #region PROPERTY
    public Rigidbody FakeWhirlwindAttackRigidBody
    {
        get => fakeWhirlwindAttackRigidBody; set => fakeWhirlwindAttackRigidBody = value;
    }

    public Collider SwordCollider
    {
        get => swordCollider; set => swordCollider = value;
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

        // _actualAttackAnimationDuration = GetActualAttackAnimationDuration();

        StartCoroutine(AutoMeleeAttack());

        _initialAttackFxAngle = attackFx.transform.eulerAngles;
    }

    private void Update()
    {
        if (!_isTest && swordCollider != null)
        {
            swordCollider.transform.eulerAngles = _lastAngle;
        }

        attackFx.transform.eulerAngles = _initialAttackFxAngle;
    }

    public override void OnDestroy()
    {
        LevelingUI.enableInput -= EnableInput;

        // _waitForEndAttackAnimationTween.Stop();
    }
    #endregion

    private void EnableInput(bool isEnable)
    {
        _isEnableInput = isEnable;
    }

    // private void Attack()
    // {
    //     if (_isAttacking)
    //     {
    //         return;
    //     }
    //     else
    //     {
    //         _isAttacking = true;
    //     }

    //     if (_animator.GetInteger("State") != 1)
    //     {
    //         _animator.SetInteger("State", 1);
    //     }

    //     // _animator.SetInteger("AttackAnimation", _attackAnimation);

    //     PlaySwordSlash();

    //     if (_waitForEndAttackAnimationTween.isAlive)
    //     {
    //         _waitForEndAttackAnimationTween.Stop();
    //     }

    //     // _waitForEndAttackAnimationTween = Tween.Delay(attackAnimation.length).OnComplete(() =>
    //     // {
    //     //     SetState(0);

    //     //     _tweens.Add(Tween.Delay(0.3f).OnComplete(() =>
    //     //     {
    //     //         _isAllowRotating = true;
    //     //         _isAttacking = false;
    //     //     }));
    //     // });

    //     _tweens.Add(Tween.Delay(delayTimeAttackHit).OnComplete(() =>
    //     {
    //         swordCollider.enabled = true;

    //         _tweens.Add(Tween.Delay(0.05f).OnComplete(() => swordCollider.enabled = false));
    //     }));

    //     // _isAllowRotating = false;
    // }

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

        SaferioTween.Delay(_actualAttackAnimationDuration, onCompletedAction: () =>
        {
            _animator.SetInteger("State", 0);

            enableRotatingEvent?.Invoke(gameObject.GetInstanceID(), true);

            _isAttacking = false;
        });

        FakeWhirlWideAttack();
    }

    private void FakeWhirlWideAttack()
    {
        StartCoroutine(FakeWhirlWideAttacking());
    }

    private IEnumerator FakeWhirlWideAttacking()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(Time.deltaTime);

        float angleRotated = 0;
        float deltaAngle = 12;

        yield return new WaitUntil(() => swordCollider != null);

        swordCollider.enabled = true;

        attackFx.Play();

        _isTest = true;

        fakeWhirlwindAttackRigidBody.AddTorque(new Vector3(0, 100, 0), ForceMode.Impulse);

        while (angleRotated < 360)
        {
            angleRotated += deltaAngle;

            yield return waitForSeconds;
        }

        fakeWhirlwindAttackRigidBody.angularVelocity = Vector3.zero;

        _lastAngle = fakeWhirlwindAttackRigidBody.transform.eulerAngles;

        swordCollider.enabled = false;

        attackFx.Stop();

        _isTest = false;
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
