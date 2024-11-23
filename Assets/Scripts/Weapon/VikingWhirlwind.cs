using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VikingWhirlwing : MonoBehaviour, IWeapon
{
    [Header("FAKE WHIRL WIND")]
    [SerializeField] private Rigidbody fakeWhirlwindAttackRigidBody;

    [Header("COLLIDER")]
    [SerializeField] private Collider weaponCollider;

    [Header("CUSTOMIZE")]
    [SerializeField, SaferioMeasurementUnit("degree/second")] private float rotationSpeedY;

    #region PRIVATE FIELD
    private bool _isAttacking;
    private Vector3 _lastAngle;
    #endregion

    #region PROPERTY
    public Rigidbody FakeWhirlwindAttackRigidBody
    {
        get => fakeWhirlwindAttackRigidBody; set => fakeWhirlwindAttackRigidBody = value;
    }
    #endregion

    #region ACTION
    public static event Action<bool> playAttackFxEvent;
    #endregion

    private void Update()
    {
        if (!_isAttacking && weaponCollider != null)
        {
            weaponCollider.transform.eulerAngles = _lastAngle;
        }
    }

    #region interface IWeapon
    public void Attack()
    {
        FakeWhirlWideAttack();
    }

    public Collider WeaponCollider
    {
        get => weaponCollider; set => weaponCollider = value;
    }
    #endregion

    private void FakeWhirlWideAttack()
    {
        StartCoroutine(FakeWhirlWideAttacking());
    }

    private IEnumerator FakeWhirlWideAttacking()
    {
        float customDeltaTime = 1 / 60f;

        WaitForSeconds waitForSeconds = new WaitForSeconds(customDeltaTime);

        float angleRotated = 0;
        float deltaAngle = rotationSpeedY * (customDeltaTime);

        yield return new WaitUntil(() => weaponCollider != null);

        weaponCollider.enabled = true;

        // playAttackFxEvent?.Invoke(true);

        _isAttacking = true;


        fakeWhirlwindAttackRigidBody.angularVelocity = new Vector3(0, rotationSpeedY * Mathf.Deg2Rad, 0);

        while (angleRotated < 120)
        {
            angleRotated += deltaAngle;

            // // to match with fx attack, replace this hard coded later
            // if (angleRotated > 90 && weaponCollider.enabled)
            // {
            //     weaponCollider.enabled = false;
            // }

            yield return waitForSeconds;
        }

        weaponCollider.enabled = false;

        fakeWhirlwindAttackRigidBody.angularVelocity = Vector3.zero;

        _lastAngle = fakeWhirlwindAttackRigidBody.transform.eulerAngles;

        playAttackFxEvent?.Invoke(false);

        _isAttacking = false;
    }
}
