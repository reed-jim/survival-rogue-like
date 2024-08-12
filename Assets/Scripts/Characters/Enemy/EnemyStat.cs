using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : CharacterStat
{
    // private float _hp = 100;

    // public int Level = 1;

    // public float HP
    // {
    //     get => _hp;
    //     set => _hp = value;
    // }

    // public void MinusHP(float value)
    // {
    //     _hp -= value;
    // }

    public void Reset()
    {
        HP = 100;
    }

    public void GetStronger(int spawnTime)
    {
        HP = 100 + 50 * spawnTime + 5 * Mathf.Pow(spawnTime, 2);
        Damage = 5 + 5 * spawnTime;
    }
}
