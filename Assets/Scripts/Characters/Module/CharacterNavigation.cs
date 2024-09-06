using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterNavigation : MonoBehaviour
{
    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private PlayerRuntime playerRuntime;

    [Header("CUSTOMIZE")]
    [SerializeField] private Vector3 offsetToPlayer;

    #region PRIVATE FIELD
    private NavMeshAgent _navMeshAgent;
    private Transform _player;
    #endregion

    #region ACTION
    public static event Action<int> enemyAttackEvent;
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

    private void Update()
    {
        if (gameObject.activeSelf && _player != null)
        {
            transform.LookAt(_player);
        }
    }

    private IEnumerator Navigating()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);

        while (true)
        {
            if (gameObject.activeSelf && _player != null)
            {
                if
                (
                    Mathf.Abs(transform.position.x - _player.position.x) > offsetToPlayer.x ||
                    Mathf.Abs(transform.position.z - _player.position.z) > offsetToPlayer.z
                )
                {
                    _navMeshAgent.isStopped = false;
                    
                    _navMeshAgent.SetDestination(_player.position + offsetToPlayer);

                    setCharacterAnimationFloatProperty?.Invoke(gameObject.GetInstanceID(), "Speed", Mathf.InverseLerp(0, 1, _navMeshAgent.velocity.magnitude));

                    yield return new WaitForSeconds(0.1f);

                    _navMeshAgent.isStopped = true;
                }
                else
                {
                    enemyAttackEvent?.Invoke(gameObject.GetInstanceID());
                }

                // if (_navMeshAgent.hasPath)
                // {
                //     if (_navMeshAgent.remainingDistance > 1)
                //     {
                //         _navMeshAgent.SetDestination(_player.position + offsetToPlayer);
                //     }
                // }
                // else
                // {
                //     _navMeshAgent.SetDestination(_player.position + offsetToPlayer);
                // }
            }

            yield return waitForSeconds;
        }
    }
}
