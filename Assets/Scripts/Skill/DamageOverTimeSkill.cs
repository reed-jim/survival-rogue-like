using System;
using UnityEngine;

[CreateAssetMenu(fileName = "skill", menuName = "ScriptableObjects/RPG/DamageSkill")]
public class DamageOverTimeSkill : ScriptableObject, ISkill
{
    [SerializeField] private StatusEffectDamaging statusEffectDamaging;

    public StatusEffectDamaging StatusEffectDamaging => statusEffectDamaging;

    #region Action
    public static event Action<DamageOverTimeSkill> addSkillEvent;
    #endregion

    // public float TotalDamage
    // {
    //     set => totalDamage = value;
    // }

    // public float Duration
    // {
    //     set => duration = value;
    // }

    // public float DamagePerSecond => totalDamage / duration;

    public string GetDescription()
    {
        return statusEffectDamaging.GetDescription();
    }

    public string GetName()
    {
        return name;
    }

    public int GetTier()
    {
        return 1;
    }

    public void AddSkill()
    {
        addSkillEvent?.Invoke(this);
    }
}
