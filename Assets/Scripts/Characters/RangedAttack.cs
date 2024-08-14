using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CustomDelegate;

public class RangedAttack : MonoBehaviour
{
    [Header("CUSTOMIZE")]
    [SerializeField] private float bulletSpeedMultiplier;

    public static event GetRigidbodyAction getRigidbodyEvent;

    public void Attack(Transform target, Transform attacker)
    {
        Rigidbody bullet = getRigidbodyEvent?.Invoke();

        bullet.gameObject.SetActive(true);

        bullet.transform.position = attacker.transform.position;

        bullet.AddForce(bulletSpeedMultiplier * (target.position - attacker.position));
    }
}
