using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class CharacterLocomotion : NetworkBehaviour
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

    public override void OnDestroy()
    {
        JoystickController.controlPlayerEvent -= ControlPlayer;
    }

    [Rpc(SendTo.Server)]
    private void SyncVelocityWithServerRpc(Vector3 velocity)
    {
        if (IsServer && !IsOwner)
        {
            // _speedMagnitude = 0.01f * _moveDirection.y;

            // _speedMagnitude = Mathf.Min(_speedMagnitude, 1);

            // characterRigidbody.velocity = speedMultiplier * _speedMagnitude * _moveDirection;

            Vector3 predictedPosition = transform.position + velocity * Time.fixedDeltaTime;

            transform.position = predictedPosition;
        }
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        characterRigidbody.velocity = speedMultiplier * _speedMagnitude * _moveDirection;

        if (!IsServer)
        {
            SyncVelocityWithServerRpc(characterRigidbody.velocity);
        }

        if (!Input.GetMouseButton(0))
        {
            if (_speedMagnitude > 0)
            {
                _speedMagnitude -= 0.4f * deltaSpeedMultiplier;
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

    // [Rpc(SendTo.NotOwner)]
    // private void ControlPlayerRpc(Vector2 joystickDirection)
    // {
    //     if (!IsOwner)
    //     {
    //         ControlPlayer(joystickDirection);
    //     }
    // }

    private void ControlPlayer(Vector2 joystickDirection)
    {
        if (!IsOwner)
        {
            return;
        }

        if (joystickDirection.y > 0)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

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
