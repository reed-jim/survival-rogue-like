using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PhotonManager : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    #region ACTION
    public static event Action<Transform> followPlayerEvent;
    #endregion

    private void Start()
    {
        GameObject player = PhotonNetwork.Instantiate(prefab.name, prefab.transform.position, Quaternion.identity);

        if (player.GetComponent<PhotonView>().IsMine)
        {
            followPlayerEvent?.Invoke(player.transform);
        }
    }
}
