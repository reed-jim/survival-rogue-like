using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerCamera : MonoBehaviour
{
    [SerializeField] private PlayerRuntime playerRuntime;

    [Header("CUSTOMIZE")]
    [SerializeField] private Vector3 offset;

    private void Awake()
    {
        PhotonManager.followPlayerEvent += FollowPlayer;
    }

    private void OnDestroy()
    {
        PhotonManager.followPlayerEvent -= FollowPlayer;
    }

    void Update()
    {
        if (playerRuntime.player == null)
        {
            return;
        }

        Vector3 position = transform.position;

        position.x = playerRuntime.player.position.x + offset.x;
        position.z = playerRuntime.player.position.z + offset.z;

        transform.position = position;
    }

    private void FollowPlayer(Transform followedPlayer)
    {
        // player = followedPlayer;
    }
}
