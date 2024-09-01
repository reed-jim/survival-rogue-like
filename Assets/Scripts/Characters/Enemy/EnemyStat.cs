using System;
using System.Collections;
using System.Collections.Generic;
using ReedJim.RPG.Stat;
using UnityEngine;

public class EnemyStat : CharacterStat
{
    public void GetStronger(int spawnTime)
    {
        SetStatValue(StatComponentNameConstant.Health, 100 + 50 * spawnTime + 5 * Mathf.Pow(spawnTime, 2));
        SetStatValue(StatComponentNameConstant.Damage, 5 + 5 * spawnTime);
    }
}
