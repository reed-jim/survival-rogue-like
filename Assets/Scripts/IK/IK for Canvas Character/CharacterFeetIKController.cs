using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterFeetIKController : MonoBehaviour
{
    [SerializeField] private Transform rightFeetPoint;

    [Header("CUSTOMIZE")]
    [SerializeField] private Vector3 feetOffset;
    [SerializeField] private float updateInterval;

    private void Awake()
    {
        StartCoroutine(Test());
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(rightFeetPoint.position + Vector3.up, rightFeetPoint.position + 2 * Vector3.down, Color.red);
    }

    private IEnumerator Test()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(updateInterval);

        while (true)
        {
            if (Physics.Raycast(rightFeetPoint.position + Vector3.up, Vector3.down, out RaycastHit hit, 2))
            {
                rightFeetPoint.position = hit.point + feetOffset;
            }

            yield return waitForSeconds;
        }
    }
}
