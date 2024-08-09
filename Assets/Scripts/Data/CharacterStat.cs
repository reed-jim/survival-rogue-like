using UnityEngine;

[System.Serializable]
public class CharacterStat
{
    [SerializeField] private int level;
    [SerializeField] private float hp;
    [SerializeField] private float damage;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float criticalChance;
    [SerializeField] private float criticalDamage;

    public delegate CharacterStat GetCharacterStatAction();

    public float HP
    {
        get => hp;
        set => hp = value;
    }

    public float Damage
    {
        get => damage;
        set => damage = value;
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

    public void MinusHP(float value)
    {
        hp -= value;
    }
}
