using System;
using System.Collections;
using System.Collections.Generic;
using ReedJim.RPG.Stat;
using UnityEngine;

public class ChainLightningComponent : MonoBehaviour, ICollide
{
    private Rigidbody _rigidbody;

    [SerializeField] private float speedMultiplier;

    #region STAT
    private int _casterInstanceId;
    private CharacterStat _stat;
    #endregion

    #region ACTION
    public static event Action<int, CharacterStat> applyDamageEvent;
    public static event Action<int> characterHitEvent;
    public static event Action<int> targetReachedEvent;
    #endregion

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Setup(int casterInstanceId, CharacterStat stat)
    {
        _casterInstanceId = casterInstanceId;
        _stat = stat;
    }

    public void ChainTo(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;

        _rigidbody.velocity = speedMultiplier * direction;
    }

    public void HandleOnCollide(GameObject other)
    {
        _rigidbody.velocity = Vector3.zero;

        applyDamageEvent?.Invoke(other.GetInstanceID(), _stat);

        characterHitEvent?.Invoke(other.GetInstanceID());

        targetReachedEvent?.Invoke(_casterInstanceId);
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
