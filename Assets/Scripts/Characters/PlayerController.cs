using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using PrimeTween;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidbody;

    [Header("ANIMATOR")]
    [SerializeField] private Animator playerAnimator;

    [Header("ANIMATION DURATION")]
    [SerializeField] private AnimationClip attackAnimation;

    [Header("COLLIDER")]
    [SerializeField] private Collider swordCollider;

    [Header("TRAIL")]
    [SerializeField] private GameObject swordTrail;

    [Header("FX")]
    [SerializeField] private ParticleSystem swordSlash;

    [Header("CUSTOMIZE")]
    [SerializeField] private float force;
    [SerializeField] private float deltaSpeed;
    [SerializeField] private float _speedMultiplier;

    [SerializeField] private float delayTimeAttackHit;

    [SerializeField] private float rotateTimeMultiplier;

    [Header("CUSTOMIZE - RANGED ATTACK")]
    [SerializeField] private LayerMask layerMaskCheckRangedAttack;

    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private PlayerRuntime playerRuntime;
    [SerializeField] private PlayerStatObserver playerStatObserver;

    [Header("MANAGEMENT")]
    private List<Tween> _tweens;
    private Coroutine _waitCoroutine;
    private bool _isTurning;
    private bool _isAttacking;
    private float _initialPositionY;
    private float _speed;

    private int _attackAnimation;

    private Tween _waitForEndAttackAnimationTween;


    private bool _isAllowRotating = true;

    public PlayerShooterController playerShooterController;

    private void Awake()
    {
        _tweens = new List<Tween>();

        _rigidbody = GetComponent<Rigidbody>();

        playerRuntime.player = transform;

        swordTrail.SetActive(false);

        swordCollider.enabled = false;

        _initialPositionY = transform.position.y;

        swordSlash.gameObject.SetActive(false);

        StartCoroutine(Shooting());
    }

    private void Update()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     _rigidbody.AddForce(force * Vector3.forward, ForceMode.Force);

        //     playerAnimator.SetInteger("State", 1);

        //     StopCoroutine(_waitCoroutine);

        //     if (_waitCoroutine == null)
        //     {
        //         _waitCoroutine = StartCoroutine(WaitFor(0.5f, () => playerAnimator.SetInteger("State", 0)));
        //     }
        // }

        if (Input.GetKey(KeyCode.W))
        {
            if (_isAttacking == false)
            {
                WalkFoward();
            }
        }
        // else if (Input.GetKeyDown(KeyCode.D))
        // {
        //     if (!_isTurning)
        //     {
        //         TransformUtil.RotateRight(transform, _tweens, onCompletedAction: () => _isTurning = false);

        //         _isTurning = true;
        //     }
        // }
        // else if (Input.GetKeyDown(KeyCode.A))
        // {
        //     if (!_isTurning)
        //     {
        //         TransformUtil.RotateLeft(transform, _tweens, onCompletedAction: () => _isTurning = false);

        //         _isTurning = true;
        //     }
        // }
        // else if (Input.GetKeyDown(KeyCode.S))
        // {
        //     if (!_isTurning)
        //     {
        //         TransformUtil.RotateBack(transform, _tweens, onCompletedAction: () => _isTurning = false);

        //         _isTurning = true;
        //     }
        // }
        else
        {
            // Idle();

            if (_speed > 0)
            {
                _speed -= deltaSpeed;
            }
            else
            {
                _speed = 0;
            }
        }

        _rigidbody.velocity = _speedMultiplier * _speed * transform.forward;

        playerAnimator.SetFloat("Speed", _speed);

        FaceToMouseCursor();

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    private void OnDestroy()
    {
        _waitForEndAttackAnimationTween.Stop();
    }

    private void FaceToMouseCursor()
    {
        if (_isAllowRotating == false)
        {
            return;
        }

        // Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // float angle = Vector3.Angle(mousePosition, transform.position);

        // transform.eulerAngles = new Vector3(0, angle, 0);

        Vector3 initialEulerAngles = transform.eulerAngles;

        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);

        transform.LookAt(hit.point);

        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        // SMOOTH
        Vector3 endEulerAngles = transform.eulerAngles;

        // if (initialEulerAngles.y >= 180)
        // {
        //     initialEulerAngles = new Vector3(0, initialEulerAngles.y - 360, 0);
        // }

        if (endEulerAngles.y < 0)
        {
            endEulerAngles = new Vector3(0, endEulerAngles.y + 360, 0);
        }

        transform.eulerAngles = initialEulerAngles;

        float deltaEulerAngle = Math.Abs(endEulerAngles.y - initialEulerAngles.y);

        if (deltaEulerAngle < 5)
        {
            return;
        }

        float duration = Math.Abs(rotateTimeMultiplier * (endEulerAngles.y - initialEulerAngles.y));

        // if (endEulerAngles.y > 180 && initialEulerAngles.y < 180)
        // {
        //     Tween tween = Tween.EulerAngles(transform, initialEulerAngles, Vector3.zero, duration: duration / 2, ease: Ease.Linear).OnComplete(() =>
        //     {
        //         _tweens.Add(Tween.EulerAngles(transform, new Vector3(0, 360, 0), endEulerAngles, duration: duration / 2, ease: Ease.Linear).OnComplete(() =>
        //         {
        //             _isAllowRotating = true;
        //         }));
        //     });

        //     _tweens.Add(tween);
        // }
        // else
        // {
        //     _tweens.Add(Tween.EulerAngles(transform, initialEulerAngles, endEulerAngles, duration: duration, ease: Ease.Linear).OnComplete(() => _isAllowRotating = true));
        // }

        StartCoroutine(SmoothRotateY(transform, initialEulerAngles.y, endEulerAngles.y));

        transform.position = new Vector3(transform.position.x, _initialPositionY, transform.position.z);

        _isAllowRotating = false;
    }

    private void WalkFoward()
    {
        // _rigidbody.AddForce(force * Vector3.forward, ForceMode.Impulse);

        // if (playerAnimator.GetInteger("State") == 1) return;

        // playerAnimator.SetInteger("State", 1);

        // if (_waitCoroutine == null)
        // {
        //     _waitCoroutine = StartCoroutine(WaitFor(1.033f, () => playerAnimator.SetInteger("State", 0)));
        // }

        if (_speed <= 1)
        {
            _speed += deltaSpeed;
        }
        else
        {
            _speed = 1;
        }
    }

    private void Idle()
    {
        if (playerAnimator.GetInteger("State") == 0) return;

        playerAnimator.SetInteger("State", 0);
    }

    private void Attack()
    {
        if (_isAttacking)
        {
            return;
        }
        else
        {
            _isAttacking = true;
        }
        // if (playerAnimator.GetInteger("State") == 1) return;

        if (playerAnimator.GetInteger("State") != 1)
        {
            playerAnimator.SetInteger("State", 1);
        }

        playerAnimator.SetInteger("AttackAnimation", _attackAnimation);

        PlaySwordSlash();

        if (_waitForEndAttackAnimationTween.isAlive)
        {
            _waitForEndAttackAnimationTween.Stop();
        }

        _waitForEndAttackAnimationTween = Tween.Delay(attackAnimation.length).OnComplete(() =>
        {
            SetState(0);

            // swordCollider.enabled = false;

            // wait for transition attack --> idle
            _tweens.Add(Tween.Delay(0.3f).OnComplete(() =>
            {
                _isAllowRotating = true;
                _isAttacking = false;
            }));
        });

        if (_attackAnimation == 0)
        {
            // swordTrail.SetActive(true);

            _tweens.Add(Tween.Delay(1f).OnComplete(() => swordTrail.SetActive(false)));
        }

        // uncomment this to use multi attack animation
        // _attackAnimation++;

        // if (_attackAnimation > 2)
        // {
        //     _attackAnimation = 0;
        // }

        _tweens.Add(Tween.Delay(delayTimeAttackHit).OnComplete(() =>
        {
            swordCollider.enabled = true;

            _tweens.Add(Tween.Delay(0.05f).OnComplete(() => swordCollider.enabled = false));
        }));

        _isAllowRotating = false;
    }

    private IEnumerator Shooting()
    {
        while (true)
        {
            // RaycastHit hit;

            // if (Physics.SphereCast(transform.position, 4, transform.forward, out hit))
            // {
            //     Debug.Log(hit);
            //     if (hit.transform.tag == Constants.ENEMY_TAG)
            //     {
            //         // Debug.LogError(hit.transform.position);

            //         playerShooterController.Shoot(hit.transform, transform);
            //     }
            // }

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 6, layerMaskCheckRangedAttack);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.transform.tag == Constants.ENEMY_TAG)
                {
                    Enemy enemy = hitCollider.transform.parent.GetComponent<Enemy>();

                    if (enemy == null)
                    {
                        continue;
                    }

                    if (enemy.State == CharacterState.DIE)
                    {
                        continue;
                    }

                    Vector3 hitPoint = Physics.ClosestPoint(transform.position, hitCollider, hitCollider.transform.position, hitCollider.transform.rotation);

                    playerShooterController.Shoot(hitCollider.transform.parent, transform);

                    break;
                }
            }

            yield return new WaitForSeconds(playerStatObserver.PlayerStat.ReloadTime);
        }
    }

    private void SetState(int state)
    {
        playerAnimator.SetInteger("State", state);
    }

    private IEnumerator WaitFor(float amount, Action onCompletedAction)
    {
        float deltaTime = Time.deltaTime;

        WaitForSeconds waitForSeconds = new WaitForSeconds(deltaTime);

        float time = 0;

        while (time < amount)
        {
            time += deltaTime;

            yield return waitForSeconds;
        }

        onCompletedAction?.Invoke();

        _waitCoroutine = null;
    }

    private IEnumerator SmoothRotateY(Transform target, float startAngle, float endAngle)
    {
        float requiredAngleRotated = 0;
        float angleRotated = 0;

        bool isClockWise = endAngle - startAngle > 0 ? true : false;
        bool isMoveAlongSmallerArc = false;

        requiredAngleRotated = Mathf.Abs(endAngle - startAngle);

        if (startAngle < 180 && endAngle > 180)
        {
            isMoveAlongSmallerArc = true;

            if (startAngle + (360 - endAngle) < requiredAngleRotated)
            {
                isClockWise = !isClockWise;

                requiredAngleRotated = startAngle + (360 - endAngle);
            }
        }

        if (startAngle > 180 && endAngle < 180)
        {
            isMoveAlongSmallerArc = true;

            if (endAngle + (360 - startAngle) < requiredAngleRotated)
            {
                isClockWise = !isClockWise;

                requiredAngleRotated = endAngle + (360 - startAngle);
            }
        }

        _isAllowRotating = false;

        while (angleRotated < requiredAngleRotated)
        {
            if (isClockWise)
            {
                target.eulerAngles += new Vector3(0, 3, 0);

                angleRotated += 3;
            }
            else
            {
                target.eulerAngles -= new Vector3(0, 3, 0);

                angleRotated += 3;
            }

            yield return new WaitForFixedUpdate();
        }

        _isAllowRotating = true;
    }

    private void PlaySwordSlash()
    {
        // swordSlash.transform.position = transform.position + new Vector3(0, 1f, 1.5f);
        swordSlash.gameObject.SetActive(true);
        swordSlash.Play();
    }
}
