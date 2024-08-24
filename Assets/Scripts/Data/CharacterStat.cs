using UnityEngine;

[System.Serializable]
public class CharacterStat : ISaveLoad
{
    [SerializeField] private int level;
    [SerializeField] private float hp;
    [SerializeField] private float maxHp = 100;
    [SerializeField] private float armor;
    [SerializeField] private float blockChance;
    [SerializeField] private float damage;
    [SerializeField] private float damageModifier;
    [SerializeField] private float criticalChance;
    [SerializeField] private float criticalDamage;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackSpeed;
    [SerializeField] protected float reloadTime = 1;
    [SerializeField] private float movementSpeed;

    [SerializeField] private float takenDamageMultiplier;
    [SerializeField] private float takenDamageCriticalChance;
    [SerializeField] private float takenDamageCriticalMultiplier;

    [SerializeField] private float percentDirectDamage;
    [SerializeField] private float percentHealthExecuted;

    public delegate CharacterStat GetCharacterStatAction();

    public float HP
    {
        get => hp;
        set => hp = value;
    }

    public float MaxHP
    {
        get => maxHp;
        set => maxHp = value;
    }

    public float Damage
    {
        get => damage;
        set => damage = value;
    }

    public float DamageMultiplier
    {
        get => damageModifier;
        set => damageModifier = value;
    }

    public float CriticalChance
    {
        get => criticalChance;
        set => criticalChance = value;
    }
    public float CriticalMultiplier
    {
        get => criticalDamage;
        set => criticalDamage = value;
    }

    public float Armor
    {
        get => armor;
        set => armor = value;
    }

    public float BlockChance
    {
        get => blockChance;
        set => blockChance = value;
    }

    public int Level
    {
        get => level;
        set => level = value;
    }

    public float AttackRange
    {
        get => attackRange;
        set => attackRange = value;
    }

    public float AttackSpeed
    {
        get => attackSpeed;
        set => attackSpeed = value;
    }

    public float ReloadTime
    {
        get => reloadTime;
        set => reloadTime = value;
    }

    public float MovementSpeed
    {
        get => movementSpeed;
        set => movementSpeed = value;
    }

    public float TakenDamageMultiplier
    {
        get => takenDamageMultiplier;
        set => takenDamageMultiplier = value;
    }

    public float TakenDamageCriticalChance
    {
        get => takenDamageCriticalChance;
        set => takenDamageCriticalChance = value;
    }

    public float TakenDamageCriticalMultiplier
    {
        get => takenDamageCriticalMultiplier;
        set => takenDamageCriticalMultiplier = value;
    }

    public float PercentDirectDamage
    {
        get => percentDirectDamage;
        set => percentDirectDamage = value;
    }

    public float PercentHealthExecuted
    {
        get => percentHealthExecuted;
        set => percentHealthExecuted = value;
    }

    public static CharacterStat operator +(CharacterStat currentStat, CharacterStat bonusStat)
    {
        CharacterStat modifiedStat = currentStat;

        modifiedStat.HP += bonusStat.HP;
        modifiedStat.Armor += bonusStat.Armor;
        modifiedStat.BlockChance += bonusStat.BlockChance;
        modifiedStat.Damage += bonusStat.Damage;
        modifiedStat.DamageMultiplier += bonusStat.DamageMultiplier;
        modifiedStat.CriticalChance += bonusStat.CriticalChance;
        modifiedStat.CriticalMultiplier += bonusStat.CriticalMultiplier;

        modifiedStat.MovementSpeed += bonusStat.MovementSpeed;

        modifiedStat.TakenDamageMultiplier += bonusStat.TakenDamageMultiplier;
        modifiedStat.TakenDamageCriticalChance += bonusStat.TakenDamageCriticalChance;
        modifiedStat.TakenDamageCriticalMultiplier += bonusStat.TakenDamageCriticalMultiplier;

        modifiedStat.PercentDirectDamage += bonusStat.PercentDirectDamage;
        modifiedStat.PercentHealthExecuted += bonusStat.PercentHealthExecuted;

        return modifiedStat;
    }

    public void Save(string key)
    {
        DataUtility.Save(Constants.STAT_DATA_FILE_NAME, key, this);
    }

    public T Load<T>(string key, T baseStat)
    {
        return DataUtility.Load(Constants.STAT_DATA_FILE_NAME, key, baseStat);
    }
}
