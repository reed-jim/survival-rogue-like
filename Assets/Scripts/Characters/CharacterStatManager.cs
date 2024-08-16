using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterStatManager : MonoBehaviour
{
    #region PRIVATE FIELD
    private CharacterStat _stat;
    #endregion

    #region ACTION
    public static event Action<int, float, float, float> setHpEvent;
    public static event Action<int, CharacterStat> addCharacterStatToListEvent;
    #endregion

    private void Awake()
    {
        CharacterDamageObserver.applyDamageEvent += TakeDamage;
        CollisionHandler.applyDamageEvent += TakeDamage;

        _stat = new CharacterStat
        {
            Level = 1,
            HP = 100,
            Damage = 10
        };
    }

    private void OnEnable()
    {
        Tween.Delay(0.5f).OnComplete(() => addCharacterStatToListEvent?.Invoke(gameObject.GetInstanceID(), _stat));

        // addCharacterStatToListEvent?.Invoke(gameObject.GetInstanceID(), _stat);
    }

    private void OnDestroy()
    {
        CharacterDamageObserver.applyDamageEvent -= TakeDamage;
        CollisionHandler.applyDamageEvent -= TakeDamage;
    }

    private void TakeDamage(int instanceId, float damage)
    {
        damage = (int)damage;

        if (gameObject.GetInstanceID() == instanceId)
        {
            float prevHp = _stat.HP;
            _stat.HP -= damage;

            setHpEvent?.Invoke(instanceId, prevHp, _stat.HP, 100);
        }
    }
}
