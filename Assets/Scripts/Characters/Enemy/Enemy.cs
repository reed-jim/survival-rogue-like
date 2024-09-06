using UnityEngine;
using System.Collections.Generic;
using PrimeTween;
using System;
using ReedJim.RPG.Stat;

public class Enemy : MonoBehaviour
{
    protected Transform player;

    [Header("STAT")]
    private EnemyStat stat;
    [SerializeField] private PredifinedCharacterStat baseStat;

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
    private ICharacterVision characterVision;

    #region ACTION
    public static event Action hitEvent;
    public static event Action<Vector3> playHitFxEvent;
    public static event Action<Vector3> playBulletHitFxEvent;
    public static event Action<EnemyStat> enemySpawnedEvent;
    public static event Action<int> enemyHitEvent;
    public static event Action<int> characterHitEvent;
    public static event Action<int> enemyDieEvent;
    public static event Action<int> enemyAttackEvent;
    public static event Action<int, string, float> setCharacterAnimationFloatProperty;
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
        _materialPropertyBlock = new MaterialPropertyBlock();

        _dissolveMaterial = transform.GetChild(0).GetComponent<Renderer>().material;

        stat = EnemyStat.Load("Enemy", baseStat.GetBaseCharacterStat()) as EnemyStat;

        _index = transform.GetSiblingIndex();
    }

    private void OnEnable()
    {
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
        // characterVision.FindEnemy();
    }
    #endregion

    private void Reset()
    {
        _characterRagdoll.EnableRagdoll(false);

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

        enemyDieEvent?.Invoke(gameObject.GetInstanceID());

        setCharacterAnimationFloatProperty?.Invoke(gameObject.GetInstanceID(), "Speed", 0);

        _characterStateManager.State = CharacterState.DIE;

        _characterRagdoll.EnableRagdoll(true);
    }
}
