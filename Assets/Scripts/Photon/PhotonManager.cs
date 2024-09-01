using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PhotonManager : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    #region PRIVATE FIELD
    private PhotonView _photonView;
    #endregion

    #region ACTION
    public static event Action<Transform> followPlayerEvent;
    #endregion

    private void Awake()
    {
        GameObject player = PhotonNetwork.Instantiate(prefab.name, prefab.transform.position, Quaternion.identity);

        _photonView = player.GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (_photonView != null)
        {
            if (_photonView.IsMine)
            {
                followPlayerEvent?.Invoke(transform);
            }
        }
    }
}
