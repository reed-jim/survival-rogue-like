using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CharacterNavigation : MonoBehaviour
{
    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private PlayerRuntime playerRuntime;

    [Header("CUSTOMIZE")]
    [SerializeField] private Vector3 offsetToPlayer;
    [SerializeField] private float delayAfterCatchedPlayer;

    #region PRIVATE FIELD
    private NavMeshAgent _navMeshAgent;
    private Transform _player;
    private bool _isNearTarget;
    #endregion

    #region ACTION
    public static event Action<int, string, float> setCharacterAnimationFloatProperty;
    #endregion

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        _player = playerRuntime.player;

        StartCoroutine(Navigating());
    }

    private IEnumerator Navigating()
    {
        Vector3 distanceToTarget = new Vector3();

        WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);
        WaitForSeconds waitDelayAfterCatchedPlayer = new WaitForSeconds(delayAfterCatchedPlayer);

        while (true)
        {
            if (gameObject.activeSelf && _player != null)
            {
                distanceToTarget.x = Mathf.Abs(transform.position.x - _player.position.x);
                distanceToTarget.z = Mathf.Abs(transform.position.z - _player.position.z);

                // avoid frequent checking target
                if (_isNearTarget)
                {
                    if
                    (
                        distanceToTarget.x > 3f * offsetToPlayer.x ||
                        distanceToTarget.z > 3f * offsetToPlayer.z
                    )
                    {
                        Debug.Log("stop");
                        _navMeshAgent.isStopped = false;

                        _isNearTarget = false;
                    }
                    else
                    {
                        yield return waitForSeconds;

                        continue;
                    }
                }

                if
                (
                    distanceToTarget.x > offsetToPlayer.x ||
                    distanceToTarget.z > offsetToPlayer.z
                )
                {
                    _navMeshAgent.SetDestination(_player.position + (_player.position - transform.position));

                    setCharacterAnimationFloatProperty?.Invoke(gameObject.GetInstanceID(), "Speed", Mathf.InverseLerp(0, 1, _navMeshAgent.velocity.magnitude));
                }
                else
                {
                    setCharacterAnimationFloatProperty?.Invoke(gameObject.GetInstanceID(), "Speed", 0);

                    _navMeshAgent.isStopped = true;

                    _isNearTarget = true;

                    // yield return waitDelayAfterCatchedPlayer;

                    // _navMeshAgent.isStopped = false;

                    // _isNearTarget = true;
                }
            }

            yield return waitForSeconds;
        }
    }
}
