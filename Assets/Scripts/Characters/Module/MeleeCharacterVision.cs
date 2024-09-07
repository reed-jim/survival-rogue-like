using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class MeleeCharacterVision : BaseCharacterVision, ICharacterVision
{
    [Header("MODULE")]
    private CharacterStateManager _characterStateManager;

    [Header("CUSTOMIZE")]
    [SerializeField] private float radiusCheck;
    [SerializeField] private LayerMask layerMaskCheck;

    private void Awake()
    {
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
            return;
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, radiusCheck, layerMaskCheck);

        if (colliders.Length > 0)
        {
            InvokeAttackEnemyEvent(gameObject.GetInstanceID(), colliders[0].transform);
        }
    }
}
