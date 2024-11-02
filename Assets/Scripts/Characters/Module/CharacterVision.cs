using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class CharacterVision : BaseCharacterVision, ICharacterVision
{
    [Header("CUSTOMIZE")]
    [SerializeField] private float radiusCheck;
    [SerializeField] private LayerMask layerMaskCheck;

    #region PRIVATE FIELD
    private List<Tween> _tweens;
    private bool _isInCountdownCheckAttackEnemy;
    private Transform _lastNearestTarget;
    #endregion  

    #region ACTION
    public static event Action<int, Transform> setTargetEvent;
    #endregion

    private void Awake()
    {
        ExplosionActiveSkill.getEnemyPositionEvent += GetTargetPosition;

        _tweens = new List<Tween>();
    }

    private void Update()
    {
        FindEnemy();
    }

    private void OnDestroy()
    {
        ExplosionActiveSkill.getEnemyPositionEvent -= GetTargetPosition;
    }

    public void FindEnemy()
    {
        if (_isInCountdownCheckAttackEnemy)
        {
            return;
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, radiusCheck, layerMaskCheck);

        if (colliders.Length > 0)
        {
            InvokeAttackEnemyEvent(gameObject.GetInstanceID(), colliders[0].transform);

            _isInCountdownCheckAttackEnemy = true;

            _tweens.Add(
                Tween.Delay(3f).OnComplete(() => _isInCountdownCheckAttackEnemy = false)
            );

            _lastNearestTarget = colliders[0].transform;

            setTargetEvent?.Invoke(gameObject.GetInstanceID(), _lastNearestTarget);
        }
    }

    private Transform GetTarget(int instanceId)
    {
        if (instanceId == gameObject.GetInstanceID() && _lastNearestTarget != null)
        {
            return _lastNearestTarget;
        }

        return null;
    }

    private Vector3 GetTargetPosition(int instanceId)
    {
        if (instanceId == gameObject.GetInstanceID() && _lastNearestTarget != null)
        {
            return _lastNearestTarget.position;
        }

        return Vector3.zero;
    }
}
