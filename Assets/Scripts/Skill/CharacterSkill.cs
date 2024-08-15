using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkill : MonoBehaviour
{
    [SerializeField] private List<DamageOverTimeSkill> damageSkills;

    public static event Action<string, DamageOverTimeSkill> applyDamageEvent;

    private void Awake()
    {
        damageSkills = new List<DamageOverTimeSkill>();

        Enemy.characterHitEvent += ApplyEffect;

        DamageOverTimeSkill testSkill = new DamageOverTimeSkill
        {
            TotalDamage = 50,
            Duration = 3
        };

        damageSkills.Add(testSkill);
    }

    private void OnDestroy()
    {
        Enemy.characterHitEvent -= ApplyEffect;
    }

    private void ApplyEffect(string instanceId)
    {
        applyDamageEvent?.Invoke(instanceId, damageSkills[0]);
    }
}
