using System;
using System.Collections;
using System.Collections.Generic;
using ReedJim.RPG.Stat;
using UnityEngine;
using static CustomDelegate;

public class MeleeWeapon : MonoBehaviour, ICollide
{
    #region PRIVATE FIELD
    protected int _attackerInstanceId;
    #endregion

    #region ACTION
    public static event Action<int, CharacterStat> applyDamageEvent;
    public static event Action<int> characterHitEvent;
    public static event GetCharacterStatAction<int> getAttackerStatAction;
    #endregion

    private void Awake()
    {
        _attackerInstanceId = transform.parent.gameObject.GetInstanceID();
    }

    protected CharacterStat GetAttackerStat()
    {
        return getAttackerStatAction.Invoke(_attackerInstanceId);
    }

    #region ICollide Implement
    public void HandleOnCollide(GameObject other)
    {
        applyDamageEvent?.Invoke(other.GetInstanceID(), GetAttackerStat());

        characterHitEvent?.Invoke(other.GetInstanceID());
    }
    #endregion
}
