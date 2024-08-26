using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private PlayerShooterController playerShooterController;

    [Header("TEMP")]
    [SerializeField] private Material sworldSlashMaterial;
    [SerializeField] private GameObject swordSlashTemp;

    [Header("COLLIDER")]
    [SerializeField] private Collider swordCollider;

    [Header("FX")]
    [SerializeField] private ParticleSystem swordSlash;

    [Header("CUSTOMIZE")]
    [SerializeField] private float delayTimeAttackHit;

    #region PRIVATE FIELD
    private Animator _animator;
    private bool _isAttacking;
    private Tween _waitForEndAttackAnimationTween;
    private List<Tween> _tweens;
    private bool _isEnableInput = true;
    #endregion

    #region LIFE CYCLE
    private void Awake()
    {
        _tweens = new List<Tween>();

        LevelingUI.enableInput += EnableInput;

        swordCollider.enabled = false;

        swordSlash.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _isEnableInput && !UIUtil.IsClickOverUI())
        {
            MeleeAttack();
        }
    }

    private void OnDestroy()
    {
        LevelingUI.enableInput -= EnableInput;

        // _waitForEndAttackAnimationTween.Stop();
    }
    #endregion

    private void EnableInput(bool isEnable)
    {
        _isEnableInput = isEnable;
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

        if (_animator.GetInteger("State") != 1)
        {
            _animator.SetInteger("State", 1);
        }

        // _animator.SetInteger("AttackAnimation", _attackAnimation);

        PlaySwordSlash();

        if (_waitForEndAttackAnimationTween.isAlive)
        {
            _waitForEndAttackAnimationTween.Stop();
        }

        // _waitForEndAttackAnimationTween = Tween.Delay(attackAnimation.length).OnComplete(() =>
        // {
        //     SetState(0);

        //     _tweens.Add(Tween.Delay(0.3f).OnComplete(() =>
        //     {
        //         _isAllowRotating = true;
        //         _isAttacking = false;
        //     }));
        // });

        _tweens.Add(Tween.Delay(delayTimeAttackHit).OnComplete(() =>
        {
            swordCollider.enabled = true;

            _tweens.Add(Tween.Delay(0.05f).OnComplete(() => swordCollider.enabled = false));
        }));

        // _isAllowRotating = false;
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

        PlaySwordSlash();

        if (_waitForEndAttackAnimationTween.isAlive)
        {
            _waitForEndAttackAnimationTween.Stop();
        }

        _tweens.Add(Tween.Delay(0.5f).OnComplete(() =>
        {
            _isAttacking = false;
        }));

        _tweens.Add(Tween.Delay(delayTimeAttackHit).OnComplete(() =>
        {
            swordCollider.enabled = true;

            _tweens.Add(Tween.Delay(0.02f).OnComplete(() => swordCollider.enabled = false));
        }));
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

    private void PlaySwordSlash()
    {
        swordSlashTemp.gameObject.SetActive(true);

        Tween.Custom(0, 1, duration: 0.8f, onValueChange: newVal => sworldSlashMaterial.SetFloat("_Slash", newVal));

        // swordSlash.Play();
    }
}
