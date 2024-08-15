using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatManager : MonoBehaviour
{
    public static event Action<string, float, float, float> setHpEvent;
    private CharacterStat _stat;

    private void Awake()
    {
        CharacterDamageObserver.applyDamageEvent += TakeDamage;

        _stat = new CharacterStat
        {
            Level = 1,
            HP = 100,
            Damage = 10
        };
    }

    private void OnDestroy()
    {
        CharacterDamageObserver.applyDamageEvent -= TakeDamage;
    }

    private void TakeDamage(string instanceId, float damage)
    {
        damage = (int)damage;

        if (gameObject.GetInstanceID().ToString() == instanceId)
        {
            float prevHp = _stat.HP;
            _stat.HP -= damage;

            setHpEvent?.Invoke(instanceId, prevHp, _stat.HP, 100);
        }
    }
}
