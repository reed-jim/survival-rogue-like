using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterSkill : MonoBehaviour
{
    [SerializeField] private List<DamageOverTimeSkill> damageSkills;

    #region ACTION
    public static event Action<int, StatusEffectDamaging[]> applyDamagingStatusEffect;
    public static event Action<CharacterStat> updatePlayerStat;
    #endregion

    #region LIFE CYCLE
    private void Awake()
    {
        damageSkills = new List<DamageOverTimeSkill>();

        CollisionHandler.characterHitEvent += ApplyEffect;
        LevelingUI.addSkillEvent += OnSkillAdded;
    }

    private void OnDestroy()
    {
        CollisionHandler.characterHitEvent -= ApplyEffect;
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
        else if (skill is DamageOverTimeSkill)
        {
            damageSkills.Add((DamageOverTimeSkill)skill);
        }
    }

    private void ApplyEffect(int instanceId)
    {
        StatusEffectDamaging[] statusEffectDamagings = new StatusEffectDamaging[damageSkills.Count];

        for (int i = 0; i < statusEffectDamagings.Length; i++)
        {
            statusEffectDamagings[i] = damageSkills[i].StatusEffectDamaging;
        }

        applyDamagingStatusEffect?.Invoke(instanceId, statusEffectDamagings);
    }
}
