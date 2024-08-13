using UnityEngine;
using System.Collections.Generic;
using PrimeTween;
using System;

public enum CharacterState
{
    NONE,
    IDLE,
    DIE,
    ATTACK
}

public class Enemy : MonoBehaviour
{
    [Header("COLLIDER")]
    [SerializeField] private Collider meleeAttackCollider;

    [Header("RAGDOLL")]
    [SerializeField] private Collider[] ragdollColliders;
    private Rigidbody[] _ragdollRigibodies;

    [SerializeField] Renderer meshRenderer;
    private Transform player;
    [SerializeField] private GameObject fx;

    [Header("ANIMATOR")]
    [SerializeField] private Animator playerAnimator;

    [Header("STAT")]
    [SerializeField] private EnemyStat stat;

    [Header("UI")]
    private CharacterUI _characterUI;

    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private PlayerStat playerStat;
    [SerializeField] private PlayerRuntime playerRuntime;

    [Header("CUSTOMIZE")]
    [SerializeField] private float speedMultiplier;
    [SerializeField] private Vector3 offsetToPlayer;

    [Header("MANAGEMENT")]
    private List<Tween> _tweens;
    private Tween _hitEffectTween;
    private CharacterState _state;
    private Rigidbody _rigidBody;
    private bool _isIgnorePhysic;
    MaterialPropertyBlock _materialPropertyBlock;
    private Material _dissolveMaterial;

    private int _index;

    #region PROPERTY
    public CharacterState State => _state;
    #endregion

    #region ACTION
    public static event Action hitEvent;
    public static event Action<Vector3> playHitFxEvent;
    public static event Action<Vector3> playBulletHitFxEvent;
    public static event Action<EnemyStat> enemySpawnedEvent;
    public static event Action<int> enemyHitEvent;
    public static event Action<int> enemyDieEvent;
    public static event Action<int> resetEnemyEvent;
    public static event Action<float> playerGotHitEvent;
    #endregion

    #region LIFE CYCLE
    private void Awake()
    {
        _tweens = new List<Tween>();

        StatManager.updateEnemyUIEvent += UpdateUI;
        EnemySpawnManager.setEnemyIndexEvent += SetIndex;

        _rigidBody = GetComponent<Rigidbody>();
        _characterUI = GetComponent<CharacterUI>();
        _materialPropertyBlock = new MaterialPropertyBlock();

        _dissolveMaterial = transform.GetChild(0).GetComponent<MeshRenderer>().material;

        stat = new EnemyStat()
        {
            HP = 100,
            Damage = 10
        };

        _index = transform.GetSiblingIndex();

        SetUpRagdoll();
        EnableRagdoll(false);
    }

    private void OnEnable()
    {
        _characterUI.ShowHpBar();

        Reset();
    }

    private void Start()
    {
        player = playerRuntime.player;

        enemySpawnedEvent?.Invoke(stat);
    }

    private void OnDestroy()
    {
        StatManager.updateEnemyUIEvent -= UpdateUI;
        EnemySpawnManager.setEnemyIndexEvent -= SetIndex;
    }

    private void Update()
    {
        FindPlayer();
    }
    #endregion

    private void Reset()
    {
        EnableRagdoll(false);

        meleeAttackCollider.gameObject.SetActive(false);

        resetEnemyEvent?.Invoke(_index);

        _characterUI.Reset();

        _state = CharacterState.NONE;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Fx Collider")
        {
            OnHit(transform.position);
        }

        if (other.tag == Constants.PLAYER_TAG)
        {
            playerGotHitEvent?.Invoke(stat.Damage);
        }

        if (other.tag == Constants.PLAYER_BULLET_TAG)
        {
            OnBulletHit(other.transform.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isIgnorePhysic || stat.HP <= 0)
        {
            return;
        }

        Vector3 hitPosition = new Vector3();

        foreach (ContactPoint contact in collision.contacts)
        {
            hitPosition = contact.point;

            break;
        }

        if (collision.collider.tag == "Sword")
        {
            OnHit(hitPosition);
        }

        if (collision.collider.tag == Constants.PLAYER_BULLET_TAG)
        {
            OnBulletHit(hitPosition);
        }
    }

    #region RAGDOLL
    private void SetUpRagdoll()
    {
        _ragdollRigibodies = new Rigidbody[ragdollColliders.Length];

        for (int i = 0; i < ragdollColliders.Length; i++)
        {
            _ragdollRigibodies[i] = ragdollColliders[i].GetComponent<Rigidbody>();

        }
    }

    public void EnableRagdoll(bool enableRagdoll)
    {
        playerAnimator.enabled = !enableRagdoll;

        foreach (Collider item in ragdollColliders)
        {
            item.enabled = false;
        }

        foreach (var ragdollRigidBody in _ragdollRigibodies)
        {
            ragdollRigidBody.useGravity = enableRagdoll;
            ragdollRigidBody.isKinematic = !enableRagdoll;
        }

        _rigidBody.velocity = Vector3.zero;
    }
    #endregion

    private void SetIndex(int index)
    {
        // _index = index;
    }

    private void OnHit(Vector3 hitPosition)
    {
        // float prevHP = stat.HP;

        enemyHitEvent?.Invoke(_index);

        // stat.MinusHP(playerStat.Damage);

        // _characterUI.SetHP(prevHP, stat.HP, maxHp: 100);

        if (stat.HP <= 0)
        {
            Die();
        }
        else
        {
            if (_hitEffectTween.isAlive)
            {
                _hitEffectTween.Stop();
            }

            float startScale = transform.localScale.x;

            _hitEffectTween = Tween.Scale(transform, 1.1f * startScale, cycles: 2, cycleMode: CycleMode.Yoyo, duration: 0.15f);

            CommonUtil.OnHitColorEffect(meshRenderer, _materialPropertyBlock,
                new Color(0.4f, 0.4f, 0.4f, 1), new Color(1, 0.4f, 0.4f, 1), 0.2f, _tweens);
        }

        // _tweens.Add(Tween.Delay(1).OnComplete(() => _isIgnorePhysic = false));

        // _isIgnorePhysic = true;

        hitEvent?.Invoke();
        playHitFxEvent?.Invoke(hitPosition);
    }

    private void OnBulletHit(Vector3 hitPosition)
    {
        enemyHitEvent?.Invoke(_index);

        if (stat.HP <= 0)
        {
            Die();
        }
        else
        {
            if (_hitEffectTween.isAlive)
            {
                _hitEffectTween.Stop();
            }

            float startScale = transform.localScale.x;

            _hitEffectTween = Tween.Scale(transform, 1.1f * startScale, cycles: 2, cycleMode: CycleMode.Yoyo, duration: 0.1f);

            CommonUtil.OnHitColorEffect(meshRenderer, _materialPropertyBlock,
                new Color(0.4f, 0.4f, 0.4f, 1), new Color(1, 0.4f, 0.4f, 1), 0.1f, _tweens);
        }

        // _tweens.Add(Tween.Delay(1).OnComplete(() => _isIgnorePhysic = false));

        // _isIgnorePhysic = true;

        hitEvent?.Invoke();
        playBulletHitFxEvent?.Invoke(hitPosition);
    }

    private void UpdateUI(int enemyIndex, float damage)
    {
        if (enemyIndex == _index)
        {
            _characterUI.SetHP(stat.HP + damage, stat.HP, maxHp: 100);
        }
    }

    private void Dissolve()
    {
        _tweens.Add(Tween.Custom(3, 0, duration: 1f, onValueChange: value => _dissolveMaterial.SetFloat("_CutoffHeight", value)).OnComplete(() =>
        {
            gameObject.SetActive(false);

            _dissolveMaterial.SetFloat("_CutoffHeight", 3);
        }));
    }

    private void FindPlayer()
    {
        if (_state == CharacterState.ATTACK)
        {
            return;
        }

        if (_state == CharacterState.DIE)
        {
            _rigidBody.velocity = Vector3.zero;

            return;
        }

        _rigidBody.velocity = Vector3.zero;

        if
        (
            Mathf.Abs(transform.position.x - player.position.x) < offsetToPlayer.x &&
            Mathf.Abs(transform.position.z - player.position.z) < offsetToPlayer.z
        )
        {
            Attack();

            return;
        }

        transform.LookAt(player);

        _rigidBody.velocity = speedMultiplier * (player.position - transform.position).normalized;

        playerAnimator.SetFloat("Speed", Math.Abs(Math.Max(_rigidBody.velocity.x, _rigidBody.velocity.z)));
    }

    private void Die()
    {
        Dissolve();

        _characterUI.HideHpBar();

        enemyDieEvent?.Invoke(stat.Level);

        playerAnimator.SetFloat("Speed", 0);

        _state = CharacterState.DIE;

        EnableRagdoll(true);
    }

    private void Attack()
    {
        playerAnimator.SetFloat("Speed", 0);
        playerAnimator.SetInteger("State", 1);

        _tweens.Add(Tween.Delay(1.3f).OnComplete(() => meleeAttackCollider.gameObject.SetActive(true)));
        _tweens.Add(Tween.Delay(5f).OnComplete(() =>
        {
            playerAnimator.SetInteger("State", 0);
            _state = CharacterState.IDLE;
        }));

        _state = CharacterState.ATTACK;
    }
}
