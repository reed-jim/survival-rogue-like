using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototypes.Common
{
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody characterRigidbody;
        [SerializeField] private float speedMultiplier;
        [SerializeField] private float autoBrakeMultiplier;

        private void Awake()
        {
            JoystickController.controlPlayerEvent += Control;
        }

        private void OnDestroy()
        {
            JoystickController.controlPlayerEvent -= Control;
        }

        private void Update()
        {
            AutoBrake();
        }

        private void AutoBrake()
        {
            Vector3 reverseForce = -1 * autoBrakeMultiplier * characterRigidbody.velocity;

            reverseForce.y = 0;

            characterRigidbody.velocity += reverseForce;
        }

        // temp, this should be in a seperate module
        private void Control()
        {
            if (Input.GetMouseButton(0))
            {
                characterRigidbody.velocity = speedMultiplier * transform.forward;
            }

            if (Input.GetKey(KeyCode.W))
            {
                characterRigidbody.velocity = speedMultiplier * transform.forward;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                characterRigidbody.velocity = -speedMultiplier * transform.right;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                characterRigidbody.velocity = -speedMultiplier * transform.forward;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                characterRigidbody.velocity = speedMultiplier * transform.right;
            }
        }

        private void Control(Vector2 normalizedDirection)
        {
            characterRigidbody.velocity = speedMultiplier * new Vector3(normalizedDirection.x, characterRigidbody.velocity.y, normalizedDirection.y);
        }
    }
}