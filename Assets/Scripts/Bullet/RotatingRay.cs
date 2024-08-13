using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingRay : MonoBehaviour
{
    [Header("DEATH RAY")]
    [SerializeField] private Transform fireRay;

    [Header("CUSTOMIZE")]
    [SerializeField] private float rotateSpeed;

    private Vector3 _prevEulerAngles;

    private void Awake()
    {
        _prevEulerAngles = fireRay.eulerAngles;
    }

    private void Update()
    {
        fireRay.eulerAngles = new Vector3(0, _prevEulerAngles.y + rotateSpeed, 0);

        _prevEulerAngles = fireRay.eulerAngles;
    }
}
