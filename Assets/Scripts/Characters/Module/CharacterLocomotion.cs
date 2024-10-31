using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLocomotion : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody characterRigidbody;

    [SerializeField] private float deltaSpeedMultiplier;
    [SerializeField] private float speedMultiplier;

    private Vector3 _moveDirection;
    private Vector3 _locomotionDirection;
    private Vector2 _joystickDirection;

    private float _speedMagnitude;

    private float damping = 3;

    private void Awake()
    {
        JoystickController.controlPlayerEvent += ControlPlayer;

        StartCoroutine(AutoRotate());
    }

    private void OnDestroy()
    {
        JoystickController.controlPlayerEvent -= ControlPlayer;
    }

    private void Update()
    {
        characterRigidbody.velocity = speedMultiplier * _speedMagnitude * _moveDirection;

        if (!Input.GetMouseButton(0))
        {
            if (_speedMagnitude > 0)
            {
                _speedMagnitude -= 0.15f * deltaSpeedMultiplier;
            }
            else
            {
                _speedMagnitude = 0;
            }

            Vector3 forward = transform.forward * _joystickDirection.y;
            Vector3 right = transform.right * _joystickDirection.x;

            _locomotionDirection = _speedMagnitude * (forward + right).normalized;

            animator.SetFloat("SpeedX", _locomotionDirection.x);
            animator.SetFloat("SpeedY", _locomotionDirection.z);
        }
    }

    private void ControlPlayer(Vector2 joystickDirection)
    {
        _speedMagnitude += deltaSpeedMultiplier;

        _speedMagnitude = Mathf.Min(_speedMagnitude, 1);

        _moveDirection = new Vector3(joystickDirection.x, 0, joystickDirection.y).normalized;
        _moveDirection.y = 0;

        _joystickDirection = joystickDirection;

        Vector3 forward = transform.forward * _joystickDirection.y;
        Vector3 right = transform.right * _joystickDirection.x;

        _locomotionDirection = _speedMagnitude * (forward + right).normalized;

        animator.SetFloat("SpeedX", _locomotionDirection.x);
        animator.SetFloat("SpeedY", _locomotionDirection.z);
    }

    private IEnumerator AutoRotate()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(5f);

        while (true)
        {
            // transform.eulerAngles += new Vector3(0, 90, 0);

            yield return waitForSeconds;
        }
    }
}
