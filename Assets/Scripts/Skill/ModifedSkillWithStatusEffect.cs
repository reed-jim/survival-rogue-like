using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "skill", menuName = "ScriptableObjects/RPG/ModifedSkillWithStatusEffect")]
public class ModifedSkillWithStatusEffect : ScriptableObject, ISkill
{
    // [SerializeField] private List<IModifierSkill> modifierSkills;
    // [SerializeField] private List<IStatusEffect> statusEffects;

    [SerializeField] private List<OffensiveSkill> modifierSkills;
    [SerializeField] private List<StatusEffectBase> statusEffects;

    public static event Action<IStatusEffect> addStatusEffectToSkillEvent;

    public void AddSkill()
    {
        foreach (var modifierSkill in modifierSkills)
        {
            modifierSkill.AddSkill();
        }

        foreach (var statusEffect in statusEffects)
        {
            addStatusEffectToSkillEvent?.Invoke(statusEffect);
        }
    }

    public string GetDescription()
    {
        string description = "";

        foreach (var item in modifierSkills)
        {
            description += $"\n{item.GetName()}";
        }

        foreach (var item in statusEffects)
        {
            description += $"\n{item.name}";
        }

        return description;
    }

    public string GetName()
    {
        return name;
    }

    public int GetTier()
    {
        return UnityEngine.Random.Range(0, 5);
    }
}
