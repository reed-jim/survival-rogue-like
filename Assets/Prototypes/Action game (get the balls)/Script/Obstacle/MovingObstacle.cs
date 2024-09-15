using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [SerializeField] private Rigidbody obstacleRigidbody;
    [Header("CUSTOMIZE")]
    [SerializeField] private float rangeMove;
    [SerializeField] private float durationEachCycle;
    [SerializeField] private float forceMultiplier;
    [SerializeField] private ForceMode forceMode;

    private void Awake()
    {
        StartCoroutine(Moving());
    }

    private IEnumerator Moving()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(durationEachCycle);

        float startPositionZ = transform.position.z;
        float endPositionZ = transform.position.z + rangeMove;

        int direction = 0;

        while (true)
        {
            if (direction == 0)
            {
                if (transform.position.z < endPositionZ)
                {
                    obstacleRigidbody.AddForce(forceMultiplier * Vector3.forward, forceMode);
                }
                else
                {
                    obstacleRigidbody.velocity = Vector3.zero;

                    direction = 1;
                }
            }
            else
            {
                if (transform.position.z > startPositionZ)
                {
                    obstacleRigidbody.AddForce(-forceMultiplier * Vector3.forward, forceMode);
                }
                else
                {
                    obstacleRigidbody.velocity = Vector3.zero;
                    
                    direction = 0;
                }
            }

            yield return waitForSeconds;
        }
    }
}
