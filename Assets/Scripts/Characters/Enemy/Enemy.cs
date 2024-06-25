using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
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

    [Header("CUSTOMIZE")]
    [SerializeField] private float speedMultiplier;

    public static event Action hitEvent;

    private void Awake()
    {
        _tweens = new List<Tween>();

        _rigidBody = GetComponent<Rigidbody>();
        _stat = GetComponent<EnemyStat>();
        _characterUI = GetComponent<CharacterUI>();
    }

    private void Update()
    {
        FindPlayer();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isIgnorePhysic)
        {
            return;
        }
        // foreach (ContactPoint contact in collision.contacts)
        // {
        //     // fx.transform.position = contact.point;
        //     // fx.SetActive(true);

        //     gameObject.SetActive(false);

        //     break;
        // }

        float prevHP = _stat.HP;

        _stat.MinusHP(0.35f);

        _characterUI.SetHP(prevHP, _stat.HP);

        if (_stat.HP <= 0)
        {
            _tweens.Add(Tween.Scale(transform, 0, duration: 0.3f).OnComplete(() =>
            {
                gameObject.SetActive(false);
            }));
        }

        _tweens.Add(Tween.Scale(transform, 1.1f, cycles: 2, cycleMode: CycleMode.Yoyo, duration: 0.15f).OnComplete(() =>
        {
            // _isIgnorePhysic = false;
        }));

        _tweens.Add(Tween.Delay(1).OnComplete(() => _isIgnorePhysic = false));

        _isIgnorePhysic = true;

        hitEvent?.Invoke();
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

    private void FindPlayer()
    {
        transform.LookAt(player);

        _rigidBody.velocity = speedMultiplier * (playerRuntime.player.position - transform.position).normalized;

        // transform.position = Vector3.Lerp(transform.position, playerRuntime.player.position + new Vector3(0, 0, 1), 0.002f);
    }
}
