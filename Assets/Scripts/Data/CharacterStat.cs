using UnityEngine;

[System.Serializable]
public class CharacterStat
{
    [SerializeField] private int level;
    [SerializeField] private float hp;
    [SerializeField] private float maxHp;
    [SerializeField] private float armor;
    [SerializeField] private float blockChance;
    [SerializeField] private float damage;
    [SerializeField] private float damageModifier;
    [SerializeField] private float criticalChance;
    [SerializeField] private float criticalDamage;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackSpeed;
    [SerializeField] protected float reloadTime = 1;

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

        return modifiedStat;
    }
}
