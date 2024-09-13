using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototypes.Common
{
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody characterRigidbody;
        [SerializeField] private float speedMultiplier;

        // private void Awake()
        // {
        //     JoystickController.controlPlayerEvent += Control;
        // }

        // private void OnDestroy()
        // {
        //     JoystickController.controlPlayerEvent -= Control;
        // }

        // private void Update()
        // {
        //     Control();
        // }

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
            characterRigidbody.velocity = speedMultiplier * new Vector3(normalizedDirection.x, 0, normalizedDirection.y);
        }
    }
}