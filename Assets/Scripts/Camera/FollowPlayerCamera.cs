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
        transform.position = player.transform.position + offset;
    }
}
