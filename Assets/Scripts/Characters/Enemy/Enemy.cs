using UnityEngine;
using System.Collections.Generic;
using PrimeTween;
using System;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject fx;

    [SerializeField] private PlayerRuntime playerRuntime;

    private Rigidbody _rigidBody;

    [Header("STAT")]
    [SerializeField] private EnemyStat stat;

    [Header("UI")]
    private CharacterUI _characterUI;

    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private PlayerStat playerStat;

    [Header("CUSTOMIZE")]
    [SerializeField] private float speedMultiplier;
    [SerializeField] private Vector3 offsetToPlayer;

    [Header("MANAGEMENT")]
    private List<Tween> _tweens;
    private bool _isIgnorePhysic;
    private Material _dissolveMaterial;

    private int _index;

    public static event Action hitEvent;
    public static event Action<Vector3> playHitFxEvent;
    public static event Action<EnemyStat> enemySpawnedEvent;
    public static event Action<int> enemyHitEvent;
    public static event Action<int> enemyDieEvent;

    private void Awake()
    {
        _tweens = new List<Tween>();

        StatManager.updateEnemyUIEvent += UpdateUI;
        EnemySpawnManager.setEnemyIndexEvent += SetIndex;

        _rigidBody = GetComponent<Rigidbody>();
        _characterUI = GetComponent<CharacterUI>();

        _dissolveMaterial = transform.GetChild(0).GetComponent<MeshRenderer>().material;

        stat = new EnemyStat();

        _index = transform.GetSiblingIndex();
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
    }

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
            Dissolve();

            _characterUI.HideHpBar();

            enemyDieEvent?.Invoke(stat.Level);
        }
        else
        {
            _tweens.Add(Tween.Scale(transform, 1.1f, cycles: 2, cycleMode: CycleMode.Yoyo, duration: 0.15f));
        }

        _tweens.Add(Tween.Delay(1).OnComplete(() => _isIgnorePhysic = false));

        _isIgnorePhysic = true;

        hitEvent?.Invoke();
        playHitFxEvent?.Invoke(hitPosition);
    }

    private void UpdateUI(int enemyIndex, float prevHP)
    {
        if (enemyIndex == _index)
        {
            _characterUI.SetHP(prevHP, stat.HP, maxHp: 100);
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
        _rigidBody.velocity = Vector3.zero;

        if (Mathf.Abs(transform.position.x - player.position.x) < offsetToPlayer.x)
        {
            return;
        }

        if (Mathf.Abs(transform.position.z - player.position.z) < offsetToPlayer.z)
        {
            return;
        }

        transform.LookAt(player);

        _rigidBody.velocity = speedMultiplier * (player.position - transform.position).normalized;

        // transform.position = Vector3.Lerp(transform.position, playerRuntime.player.position + new Vector3(0, 0, 1), 0.002f);
    }
}
