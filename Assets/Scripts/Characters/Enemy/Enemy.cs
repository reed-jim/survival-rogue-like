using UnityEngine;
using System.Collections.Generic;
using PrimeTween;
using System;
using ReedJim.RPG.Stat;
using ExitGames.Client.Photon.StructWrapping;
using Saferio.Util.SaferioTween;

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

    [Header("MODULE")]
    private CharacterStateManager _characterStateManager;
    private ICharacterVision _characterVision;
    private CharacterRagdoll _characterRagdoll;

    #region PRIVATE FIELD
    protected List<Tween> _tweens;
    private Material _dissolveMaterial;
    private List<Collider> _colliders;
    #endregion

    #region ACTION
    public static event Action hitEvent;
    public static event Action<Vector3> playHitFxEvent;
    public static event Action<Vector3> playBulletHitFxEvent;
    public static event Action<EnemyStat> enemySpawnedEvent;
    public static event Action<int> enemyHitEvent;
    public static event Action<int> characterHitEvent;
    public static event Action<int> enemyDieEvent;
    public static event Action<int> enemyAttackEvent;
    public static event Action<int, string, int> setCharacterAnimationIntProperty;
    public static event Action<int, string, float> setCharacterAnimationFloatProperty;
    #endregion

    #region LIFE CYCLE
    private void Awake()
    {
        _tweens = new List<Tween>();

        CharacterStatManager.characterDieEvent += Die;
        _characterStateManager = GetComponent<CharacterStateManager>();
        _characterVision = GetComponent<ICharacterVision>();
        _characterUI = GetComponent<CharacterUI>();
        _characterRagdoll = GetComponent<CharacterRagdoll>();

        _dissolveMaterial = transform.GetChild(0).GetComponent<Renderer>().material;

        stat = EnemyStat.Load("Enemy", baseStat.GetBaseCharacterStat()) as EnemyStat;

        FindAllColliders();
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

    private void Update()
    {
        // _characterVision.FindEnemy();
    }

    private void OnDestroy()
    {
        CharacterStatManager.characterDieEvent -= Die;
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
        if (_characterStateManager.State == CharacterState.DIE)
        {
            return;
        }

        _characterUI.HideHpBar();

        enemyDieEvent?.Invoke(gameObject.GetInstanceID());

        setCharacterAnimationIntProperty?.Invoke(gameObject.GetInstanceID(), "State", Constants.ANIMATION_DIE_STATE);
        setCharacterAnimationFloatProperty?.Invoke(gameObject.GetInstanceID(), "Speed", 0);

        SaferioTween.Delay(2f, onCompletedAction: () => Dissolve());

        // DisableAllColliders();

        _characterStateManager.State = CharacterState.DIE;

        _characterRagdoll.EnableRagdoll(true);
    }

    private void FindAllColliders()
    {
        _colliders = new List<Collider>();

        for (int i = 0; i < transform.childCount; i++)
        {
            Collider collider = transform.GetChild(i).GetComponent<Collider>();

            if (collider != null)
            {
                _colliders.Add(collider);
            }
        }
    }

    private void DisableAllColliders()
    {
        foreach (var item in _colliders)
        {
            item.gameObject.SetActive(false);
        }
    }
}
