using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class CharacterVision : MonoBehaviour, ICharacterVision
{
    [Header("CUSTOMIZE")]
    [SerializeField] private float radiusCheck;
    [SerializeField] private LayerMask layerMaskCheck;

    #region PRIVATE FIELD
    private List<Tween> _tweens;
    private bool _isInCountdownCheckAttackEnemy;
    #endregion  

    public static event Action<int, Transform> attackEnemyEvent;

    private void Awake()
    {
        _tweens = new List<Tween>();
    }

    private void Update()
    {
        FindEnemy();
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
            attackEnemyEvent?.Invoke(gameObject.GetInstanceID(), colliders[0].transform);

            _isInCountdownCheckAttackEnemy = true;

            _tweens.Add(
                Tween.Delay(3f).OnComplete(() => _isInCountdownCheckAttackEnemy = false)
            );
        }
    }
}
