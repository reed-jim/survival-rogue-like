using System.Collections;
using UnityEngine;

namespace Prototypes.Action
{
    public class FollowPlayerCamera : MonoBehaviour
    {
        [SerializeField] private PlayerRuntime playerRuntime;

        [SerializeField] private Vector3 offset;
        public float smoothSpeed = 0.125f;

        private Transform _player;

        private void Awake()
        {
            StartCoroutine(AssignPlayer());
        }

        private IEnumerator AssignPlayer()
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(0.02f);

            yield return new WaitUntil(() => playerRuntime.Player != null);

            _player = playerRuntime.Player;

            while (true)
            {
                transform.position = Vector3.Lerp(transform.position, _player.position + offset, smoothSpeed);

                yield return waitForSeconds;
            }
        }
    }
}
