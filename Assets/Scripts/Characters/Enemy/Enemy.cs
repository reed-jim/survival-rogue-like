using UnityEngine;
using System.Collections.Generic;
using PrimeTween;
using System;

public class Enemy : MonoBehaviour
{
    [Header("COLLIDER")]
    [SerializeField] private Collider meleeAttackCollider;

    [SerializeField] Renderer meshRenderer;
    protected Transform player;

    [Header("STAT")]
    [SerializeField] private EnemyStat stat;

    [Header("UI")]
    private CharacterUI _characterUI;

    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private PlayerRuntime playerRuntime;

    [Header("CUSTOMIZE")]
    [SerializeField] private float speedMultiplier;
    [SerializeField] private Vector3 offsetToPlayer;

    [Header("MANAGEMENT")]
    protected List<Tween> _tweens;
    private Tween _hitEffectTween;
    private Rigidbody _rigidBody;
    MaterialPropertyBlock _materialPropertyBlock;
    private Material _dissolveMaterial;
    private int _index;

    [Header("MODULE")]
    private CharacterStateManager _characterStateManager;
    private CharacterRagdoll _characterRagdoll;
    private CharacterAttack _characterAttack;

    #region ACTION
    public static event Action hitEvent;
    public static event Action<Vector3> playHitFxEvent;
    public static event Action<Vector3> playBulletHitFxEvent;
    public static event Action<EnemyStat> enemySpawnedEvent;
    public static event Action<int> enemyHitEvent;
    public static event Action<string> characterHitEvent;
    public static event Action<int> enemyDieEvent;
    public static event Action<string, float> setCharacterAnimationFloatProperty;
    #endregion

    #region LIFE CYCLE
    private void Awake()
    {
        _tweens = new List<Tween>();

        CharacterStatManager.characterDieEvent += Die;

        _rigidBody = GetComponent<Rigidbody>();
        _characterUI = GetComponent<CharacterUI>();
        _characterRagdoll = GetComponent<CharacterRagdoll>();
        _characterStateManager = GetComponent<CharacterStateManager>();
        _characterAttack = GetComponent<CharacterAttack>();
        _materialPropertyBlock = new MaterialPropertyBlock();

        _dissolveMaterial = transform.GetChild(0).GetComponent<MeshRenderer>().material;

        stat = new EnemyStat()
        {
            Level = 1,
            HP = 100,
            Damage = 10
        };

        _index = transform.GetSiblingIndex();
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
        CharacterStatManager.characterDieEvent -= Die;
    }

    private void Update()
    {
        FindPlayer();
    }
    #endregion

    private void Reset()
    {
        _characterRagdoll.EnableRagdoll(false);

        meleeAttackCollider.gameObject.SetActive(false);

        _characterUI.Reset();

        _characterStateManager.State = CharacterState.NONE;
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
        if (_characterStateManager.State == CharacterState.ATTACK)
        {
            return;
        }

        if (_characterStateManager.State == CharacterState.DIE)
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

        setCharacterAnimationFloatProperty?.Invoke("Speed", Math.Abs(Math.Max(_rigidBody.velocity.x, _rigidBody.velocity.z)));
    }

    private void Die(int instanceId)
    {
        if (instanceId == gameObject.GetInstanceID())
        {
            Die();
        }
    }

    private void Die()
    {
        Dissolve();

        _characterUI.HideHpBar();

        enemyDieEvent?.Invoke(stat.Level);

        setCharacterAnimationFloatProperty?.Invoke("Speed", 0);

        _characterStateManager.State = CharacterState.DIE;

        _characterRagdoll.EnableRagdoll(true);
    }

    protected virtual void Attack()
    {
        _characterAttack.MeleeAttack();
    }

















    private void OnHit(Vector3 hitPosition)
    {
        // float prevHP = stat.HP;

        enemyHitEvent?.Invoke(_index);
        characterHitEvent?.Invoke(gameObject.GetInstanceID().ToString());

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
}
