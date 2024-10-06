using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
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
    private List<Tween> _tweens;
    private NavMeshAgent _navMeshAgent;
    private Transform _player;
    private bool _isNearTarget;
    private bool _isAvoidAllies;
    #endregion

    #region ACTION
    public static event Action<int, string, float> setCharacterAnimationFloatProperty;
    #endregion

    private void Awake()
    {
        FlockAvoidance.avoidNeighbourEvent += StopNavigating;

        _tweens = new List<Tween>();

        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        _player = playerRuntime.player;

        StartCoroutine(Navigating());
    }

    private void OnDestroy()
    {
        FlockAvoidance.avoidNeighbourEvent -= StopNavigating;

        CommonUtil.StopAllTweens(_tweens);
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
                if (_isAvoidAllies)
                {
                    yield return waitForSeconds;

                    continue;
                }

                if (_isNearTarget)
                {
                    // transform.LookAt(_player);

                    if
                    (
                        distanceToTarget.x > 3f * offsetToPlayer.x ||
                        distanceToTarget.z > 3f * offsetToPlayer.z
                    )
                    {
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
                    _navMeshAgent.SetDestination(_player.position + 0.5f * (transform.position - _player.position));

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

    private void StopNavigating(int instanceId)
    {
        if (instanceId == gameObject.GetInstanceID())
        {
            if (!_navMeshAgent.isStopped)
            {
                _navMeshAgent.isStopped = true;
            }

            setCharacterAnimationFloatProperty?.Invoke(gameObject.GetInstanceID(), "Speed", 0);

            _isAvoidAllies = true;

            _tweens.Add(Tween.Delay(3f).OnComplete(() =>
            {
                _isAvoidAllies = false;

                if (!_isNearTarget)
                {
                    _navMeshAgent.isStopped = false;
                }
            }));
        }
    }
}
