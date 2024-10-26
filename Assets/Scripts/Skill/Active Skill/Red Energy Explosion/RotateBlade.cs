using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using ReedJim.RPG.Stat;
using Saferio.Util.SaferioTween;
using UnityEngine;
using static CustomDelegate;

public class RotateBlade : MonoBehaviour, ICollide
{
    [SerializeField] private Rigidbody rotateBladeRigibody;

    [Header("CUSTOMIZE")]
    [SerializeField] private Vector3 rotationSpeed;
    [SerializeField] private PredifinedCharacterStat stat;

    #region PRIVATE FIELD
    // private Rigidbody _rigidBody;
    private CharacterStat _skillStat;
    private Vector3 _lastAngle;
    #endregion

    #region ACTION
    public static GetExplosiveAreaIndicatorAction getExplosiveAreaIndicatorAction;
    public static event Action<int, CharacterStat> applyDamageEvent;
    public static event Action<int> characterHitEvent;
    public static event GetCharacterStatAction<int> getAttackerStatAction;
    #endregion

    private void Awake()
    {
        rotateBladeRigibody.angularVelocity = rotationSpeed;

        _skillStat = stat.GetBaseCharacterStat();
    }

    // private void Update()
    // {
    //     transform.localEulerAngles -= transform.parent.eulerAngles;
    // }

    private void FixedUpdate()
    {
        transform.eulerAngles = _lastAngle;
    }

    private void LateUpdate()
    {
        _lastAngle = transform.eulerAngles;
    }

    // private async void Rotating() {
    //     while(true) {
    //         rotateBladeRigibody.angularVelocity = 
    //     }
    // }


    public void SetStat(CharacterStat stat)
    {
        _skillStat = stat;
    }

    #region ICollider IMPLEMENT
    public void HandleOnCollide(GameObject other)
    {
        applyDamageEvent?.Invoke(other.GetInstanceID(), _skillStat);

        characterHitEvent?.Invoke(other.GetInstanceID());
    }
    #endregion
}
