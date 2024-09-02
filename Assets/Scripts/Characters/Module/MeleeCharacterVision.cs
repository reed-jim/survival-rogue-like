using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCharacterVision : MonoBehaviour, ICharacterVision
{
    [Header("MODULE")]
    private Rigidbody _rigidBody;
    private CharacterStateManager _characterStateManager;

    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private PlayerRuntime playerRuntime;

    [Header("CUSTOMIZE")]
    [SerializeField] private float speedMultiplier;
    [SerializeField] private Vector3 offsetToPlayer;

    #region PRIVATE FIELD
    private Transform _player;
    #endregion

    #region ACTION
    public static event Action<int> enemyAttackEvent;
    public static event Action<int, string, float> setCharacterAnimationFloatProperty;
    #endregion

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _characterStateManager = GetComponent<CharacterStateManager>();
    }

    public void FindEnemy()
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
            Mathf.Abs(transform.position.x - _player.position.x) < offsetToPlayer.x &&
            Mathf.Abs(transform.position.z - _player.position.z) < offsetToPlayer.z
        )
        {
            InvokeAttackEvent();

            return;
        }

        transform.LookAt(_player);

        transform.eulerAngles = TransformUtil.GetMaintainedXEulerAngle(transform);

        _rigidBody.velocity = speedMultiplier * (_player.position - transform.position).normalized;

        setCharacterAnimationFloatProperty?.Invoke(gameObject.GetInstanceID(), "Speed", Mathf.Abs(Mathf.Max(_rigidBody.velocity.x, _rigidBody.velocity.z)));
    }

    private void InvokeAttackEvent()
    {
        enemyAttackEvent?.Invoke(gameObject.GetInstanceID());
    }
}
