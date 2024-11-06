using System;
using Saferio.Util.SaferioTween;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private PlayerRuntime playerRuntime;

    // #region ACTION
    // public static CustomDelegate.GetCharacterStatAction getStatEvent;
    // public static event Action<float> playerGotHitEvent;
    // #endregion

    #region LIFE CYCLE
    private void Awake()
    {
        playerRuntime.player = transform;
        playerRuntime.PlayerInstanceId = gameObject.GetInstanceID();
    }
    #endregion
}
