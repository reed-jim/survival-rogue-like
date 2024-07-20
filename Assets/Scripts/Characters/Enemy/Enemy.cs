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
    private EnemyStat _stat;

    [Header("UI")]
    private CharacterUI _characterUI;

    [Header("MANAGEMENT")]
    private List<Tween> _tweens;
    private bool _isIgnorePhysic;
    private Material _dissolveMaterial;

    [Header("CUSTOMIZE")]
    [SerializeField] private float speedMultiplier;

    public static event Action hitEvent;
    public static event Action<Vector3> playHitFxEvent;

    private void Awake()
    {
        _tweens = new List<Tween>();

        _rigidBody = GetComponent<Rigidbody>();
        _stat = GetComponent<EnemyStat>();
        _characterUI = GetComponent<CharacterUI>();

        _dissolveMaterial = transform.GetChild(0).GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        FindPlayer();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isIgnorePhysic || _stat.HP <= 0)
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

    private void OnHit(Vector3 hitPosition)
    {
        float prevHP = _stat.HP;

        _stat.MinusHP(0.35f);

        _characterUI.SetHP(prevHP, _stat.HP);

        if (_stat.HP <= 0)
        {
            Dissolve();

            _characterUI.HideHpBar();

            // _tweens.Add(Tween.Scale(transform, 0, duration: 1f).OnComplete(() =>
            // {
            //     gameObject.SetActive(false);
            // }));
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

    // private void OnTriggerEnter(Collider other)
    // {
    //     float prevHP = _stat.HP;

    //     _stat.MinusHP(0.35f);

    //     _characterUI.SetHP(prevHP, _stat.HP);

    //     if (_stat.HP <= 0)
    //     {
    //         gameObject.SetActive(false);
    //     }
    // }

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
        transform.LookAt(player);

        _rigidBody.velocity = speedMultiplier * (playerRuntime.player.position - transform.position).normalized;

        // transform.position = Vector3.Lerp(transform.position, playerRuntime.player.position + new Vector3(0, 0, 1), 0.002f);
    }
}
