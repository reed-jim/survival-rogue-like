using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkill : MonoBehaviour
{
    [SerializeField] private List<DamageOverTimeSkill> damageSkills;

    #region ACTION
    public static event Action<string, DamageOverTimeSkill> applyDamageEvent;
    public static event Action<CharacterStat> updatePlayerStat;
    #endregion

    #region LIFE CYCLE
    private void Awake()
    {
        damageSkills = new List<DamageOverTimeSkill>();

        Enemy.characterHitEvent += ApplyEffect;
        LevelingUI.addSkillEvent += OnSkillAdded;

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
        LevelingUI.addSkillEvent -= OnSkillAdded;
    }
    #endregion

    private void OnSkillAdded(ISkill skill)
    {
        if (skill is IModifierSkill)
        {
            IModifierSkill modifierSkill = skill as IModifierSkill;

            updatePlayerStat?.Invoke(modifierSkill.GetBonusStat());
        }
    }

    private void ApplyEffect(string instanceId)
    {
        // applyDamageEvent?.Invoke(instanceId, damageSkills[0]);
    }
}
