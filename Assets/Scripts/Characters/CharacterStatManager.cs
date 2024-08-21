using System;
using PrimeTween;
using UnityEngine;

public class CharacterStatManager : MonoBehaviour
{
    #region PRIVATE FIELD
    protected CharacterStat _stat;
    #endregion

    public virtual CharacterStat Stat
    {
        get => _stat;
    }

    #region ACTION
    public static event Action<int, float, float, float> setHpEvent;
    public static event Action<int, CharacterStat> addCharacterStatToListEvent;
    public static event Action<int> characterDieEvent;
    #endregion

    private void Awake()
    {
        RegisterEvent();

        InitializeStat();
    }

    protected virtual void OnEnable()
    {
        Tween.Delay(0.5f).OnComplete(() => addCharacterStatToListEvent?.Invoke(gameObject.GetInstanceID(), Stat));
    }

    private void OnDestroy()
    {
        UnregisterEvent();
    }

    private void RegisterEvent()
    {
        // CharacterDamageObserver.applyDamageEvent += TakeDamage;
        CollisionHandler.applyDamageEvent += TakeDamage;
    }

    private void UnregisterEvent()
    {
        // CharacterDamageObserver.applyDamageEvent -= TakeDamage;
        CollisionHandler.applyDamageEvent -= TakeDamage;
    }

    protected virtual void InitializeStat()
    {
        _stat = new CharacterStat
        {
            Level = 1,
            HP = 100,
            Damage = 10
        };
    }

    private void TakeDamage(int instanceId, CharacterStat attackerStat)
    {
        if (gameObject.GetInstanceID() == instanceId)
        {
            int intDamage = DamageCalculator.GetDamage(attackerStat, _stat);

            float prevHp = Stat.HP;

            MinusHP(intDamage);

            InvokeUpdateHPBarEvent(prevHp);

            if (Stat.HP <= 0)
            {
                characterDieEvent?.Invoke(instanceId);
            }
        }
    }

    protected virtual void MinusHP(int damage)
    {
        _stat.HP -= damage;
    }

    protected virtual void InvokeUpdateHPBarEvent(float prevHp)
    {
        setHpEvent?.Invoke(gameObject.GetInstanceID(), prevHp, Stat.HP, 100);
    }
}
