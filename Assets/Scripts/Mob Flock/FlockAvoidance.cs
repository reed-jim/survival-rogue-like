using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockAvoidance : MonoBehaviour
{
    [Header("CUSTOMIZE")]
    [SerializeField] private float radiusCheck;
    [SerializeField] private LayerMask layerMaskCheck;

    private bool _isInCountdown;

    public static event Action<int> avoidNeighbourEvent;

    private void OnEnable()
    {
        StartCoroutine(Avoiding());
    }

    private void Avoid()
    {
        Collider[] neighbors = Physics.OverlapSphere(transform.position, radiusCheck, layerMaskCheck);

        bool isFoundSelf = true;

        if (neighbors.Length > 0)
        {
            foreach (var item in neighbors)
            {
                if (!item.transform.IsChildOf(transform))
                {
                    isFoundSelf = false;

                    break;
                }
            }

            if (isFoundSelf)
            {
                return;
            }

            // if (neighbors.Length == 1 && neighbors[0].transform.IsChildOf(transform))
            // {
            //     return;
            // }

            avoidNeighbourEvent?.Invoke(gameObject.GetInstanceID());

            _isInCountdown = true;
        }
    }

    private IEnumerator Avoiding()
    {
        float delayBeforeChecking = 3f;
        float durationEachStep = 0.25f;

        WaitForSeconds waitForSeconds = new WaitForSeconds(durationEachStep);

        float countDownAfterAvoid = 0;

        yield return new WaitForSeconds(delayBeforeChecking);

        while (true)
        {
            if (!_isInCountdown)
            {
                Avoid();
            }
            else
            {
                countDownAfterAvoid += durationEachStep;

                if (countDownAfterAvoid > 3f)
                {
                    _isInCountdown = false;

                    countDownAfterAvoid = 0;
                }
            }

            yield return waitForSeconds;
        }
    }
}
