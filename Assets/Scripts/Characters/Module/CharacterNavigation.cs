using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using Saferio.TreeBehaviour;
using Saferio.Util.SaferioTween;
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

    #region COROUTINE
    private Coroutine _navigatingWithTreeBehaviour;
    #endregion

    #region ACTION
    public static event Action<int, string, float> setCharacterAnimationFloatProperty;
    public static event Action<int> targetFoundEvent;
    #endregion

    private void Awake()
    {
        FlockAvoidance.avoidNeighbourEvent += StopNavigating;
        SeekTargetNode.startSeekTargetBahaviourEvent += StartNavigatingWithTreeBehaviour;
        CharacterStatManager.characterDieEvent += HandleOnCharacterDied;
        CollisionHandler.disableNavMeshEvent += DisableNavMesh;

        _tweens = new List<Tween>();

        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        _player = playerRuntime.player;

        // StartCoroutine(Navigating());
    }

    private void OnDestroy()
    {
        FlockAvoidance.avoidNeighbourEvent -= StopNavigating;
        SeekTargetNode.startSeekTargetBahaviourEvent -= StartNavigatingWithTreeBehaviour;
        CharacterStatManager.characterDieEvent -= HandleOnCharacterDied;
        CollisionHandler.disableNavMeshEvent -= DisableNavMesh;

        CommonUtil.StopAllTweens(_tweens);
    }

    private void StartNavigatingWithTreeBehaviour(int instanceId)
    {
        if (instanceId == gameObject.GetInstanceID() && gameObject.activeSelf)
        {
            _navigatingWithTreeBehaviour = StartCoroutine(NavigatingWithTreeBehaviour());

            _navMeshAgent.isStopped = false;
        }
    }

    private IEnumerator NavigatingWithTreeBehaviour()
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

                if (_isAvoidAllies)
                {
                    yield return waitForSeconds;

                    continue;
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

                    targetFoundEvent?.Invoke(gameObject.GetInstanceID());

                    yield break;
                }
            }

            yield return waitForSeconds;
        }
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

            SaferioTween.Delay(2f, onCompletedAction: () =>
            {
                _isAvoidAllies = false;

                if (!_isNearTarget)
                {
                    if (_navMeshAgent.isActiveAndEnabled)
                    {
                        _navMeshAgent.isStopped = false;
                    }
                }
            });
        }
    }

    private void HandleOnCharacterDied(int instanceId)
    {
        if (instanceId == gameObject.GetInstanceID())
        {
            _navMeshAgent.isStopped = true;

            if (_navigatingWithTreeBehaviour != null)
            {
                StopCoroutine(_navigatingWithTreeBehaviour);
            }
        }
    }

    private void DisableNavMesh(int instanceId)
    {
        if (instanceId == GetInstanceID())
        {
            _navMeshAgent.isStopped = true;

            SaferioTween.Delay(1.5f, onCompletedAction: () => _navMeshAgent.isStopped = false);
        }
    }

    private void Stun()
    {
        _navMeshAgent.isStopped = false;
    }
}
