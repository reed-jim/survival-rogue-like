using System;
using PrimeTween;
using ReedJim.RPG.Stat;
using UnityEngine;

public class CharacterStatManager : MonoBehaviour
{
    [Header("STAT")]
    [SerializeField] protected PredifinedCharacterStat baseStat;

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
    public static event Action<int> showCriticalDamageEvent;
    #endregion

    private void Awake()
    {
        RegisterEvent();

        InitializeStat();
    }

    // protected virtual void OnEnable()
    // {
    //     Tween.Delay(0.5f).OnComplete(() => addCharacterStatToListEvent?.Invoke(gameObject.GetInstanceID(), Stat));
    // }

    private void OnDestroy()
    {
        UnregisterEvent();
    }

    // private void OnApplicationQuit()
    // {
    //     _stat.Save(gameObject.name);
    // }

    private void RegisterEvent()
    {
        // CharacterDamageObserver.applyDamageEvent += TakeDamage;
        MeleeWeaponCollider.applyDamageEvent += TakeDamage;
        Meteor.applyDamageEvent += TakeDamage;
        Bullet.applyDamageEvent += TakeDamage;
        EnergyExplosion.applyDamageEvent += TakeDamage;
        RotateBlade.applyDamageEvent += TakeDamage;
        ChainLightningComponent.applyDamageEvent += TakeDamage;
        CharacterStatusEffectObserver.applyDamageEvent += TakeDamage;
        EnemySpawnManager.spawnEnemyEvent += OnEnemySpawnEvent;
    }

    private void UnregisterEvent()
    {
        // CharacterDamageObserver.applyDamageEvent -= TakeDamage;
        MeleeWeaponCollider.applyDamageEvent -= TakeDamage;
        Meteor.applyDamageEvent -= TakeDamage;
        Bullet.applyDamageEvent -= TakeDamage;
        EnergyExplosion.applyDamageEvent -= TakeDamage;
        RotateBlade.applyDamageEvent -= TakeDamage;
        ChainLightningComponent.applyDamageEvent -= TakeDamage;
        CharacterStatusEffectObserver.applyDamageEvent -= TakeDamage;
        EnemySpawnManager.spawnEnemyEvent -= OnEnemySpawnEvent;
    }

    protected virtual void InitializeStat()
    {
        // _stat = new CharacterStat
        // {
        //     Level = 1,
        //     HP = 100,
        //     Damage = 10
        // };

        _stat = CharacterStat.Load(gameObject.name, baseStat.GetBaseCharacterStat());

        addCharacterStatToListEvent?.Invoke(gameObject.GetInstanceID(), Stat);

        // Tween.Delay(0.5f).OnComplete(() =>
        // {
        //     _stat = CharacterStat.Load(gameObject.name, baseStat.GetBaseCharacterStat());

        //     addCharacterStatToListEvent?.Invoke(gameObject.GetInstanceID(), Stat);
        // });
    }

    private void TakeDamage(int instanceId, CharacterStat attackerStat)
    {
        if (gameObject.GetInstanceID() == instanceId)
        {
            DamageCalculator basicDamageCalculator = new DamageCalculator();

            int intDamage = basicDamageCalculator.GetDamage(attackerStat, Stat);

            if (basicDamageCalculator.IsCritical(intDamage, attackerStat))
            {
                showCriticalDamageEvent?.Invoke(instanceId);
            }

            // float prevHp = Stat.HP;
            float prevHp = Stat.GetStatValue(StatComponentNameConstant.Health);

            MinusHP(intDamage);

            InvokeUpdateHPBarEvent(prevHp);

            if (Stat.GetStatValue(StatComponentNameConstant.Health) <= 0)
            {
                characterDieEvent?.Invoke(instanceId);
            }
        }
    }

    protected virtual void MinusHP(int damage)
    {
        Stat.ModifyStat(StatComponentNameConstant.Health, new MinusStatModifier(), damage);
    }

    protected virtual void InvokeUpdateHPBarEvent(float prevHp)
    {
        setHpEvent?.Invoke(gameObject.GetInstanceID(), prevHp, Stat.GetStatValue(StatComponentNameConstant.Health), Stat.GetStatBaseValue(StatComponentNameConstant.Health));
    }

    private void OnEnemySpawnEvent(int instanceId)
    {
        if (instanceId == gameObject.GetInstanceID())
        {
            IStatComponent health = Stat.GetStat(StatComponentNameConstant.Health);

            if (health.Value != health.BaseValue)
            {
                InvokeUpdateHPBarEvent(health.Value);
            }
        }
    }
}
