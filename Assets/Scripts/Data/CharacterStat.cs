using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CharacterStat")]
public class CharacterStat : ScriptableObject
{
    [SerializeField] private int level;
    [SerializeField] private float hp;
    [SerializeField] private float damage;

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

    public void MinusHP(float value)
    {
        hp -= value;
    }
}
