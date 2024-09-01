using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private PlayerRuntime playerRuntime;

    [Header("MANAGEMENT")]
    private bool _isAllowRotating = true;

    // #region ACTION
    // public static CustomDelegate.GetCharacterStatAction getStatEvent;
    // public static event Action<float> playerGotHitEvent;
    // #endregion

    #region LIFE CYCLE
    private void Awake()
    {
        playerRuntime.player = transform;
    }
    #endregion
}
