using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTimeSkill : Skill
{
    [SerializeField] private float totalDamage;
    [SerializeField] private float duration;

    public float TotalDamage
    {
        set => totalDamage = value;
    }

    public float Duration
    {
        set => duration = value;
    }

    public float DamagePerSecond => totalDamage / duration;
}
