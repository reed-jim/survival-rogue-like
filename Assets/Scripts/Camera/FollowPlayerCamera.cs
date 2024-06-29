using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform player;

    [Header("CUSTOMIZE")]
    [SerializeField] private Vector3 offset;

    void Update()
    {
        Vector3 position = transform.position;

        position.x = player.transform.position.x + offset.x;
        position.z = player.transform.position.z + offset.z;

        transform.position = position;
    }
}
