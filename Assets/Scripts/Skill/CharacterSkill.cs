using System;
using System.Collections;
using System.Collections.Generic;
using ReedJim.RPG.Stat;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterSkill : MonoBehaviour
{
    [SerializeField] private ActiveSkillContainer activeSkillContainer;
    [SerializeField] private List<DamageOverTimeSkill> damageSkills;
    [SerializeField] private List<IStatusEffect> statusEffectFromSkills;

    #region ACTION
    public static event Action<int, StatusEffectDamaging[]> applyDamagingStatusEffect;
    public static event Action<CharacterStat> updatePlayerStat;
    #endregion

    #region LIFE CYCLE
    private void Awake()
    {
        damageSkills = new List<DamageOverTimeSkill>();

        CollisionHandler.characterHitEvent += ApplyEffect;
        Bullet.characterHitEvent += ApplyEffect;
        LevelingUI.addSkillEvent += OnSkillAdded;
        DamageOverTimeSkill.addSkillEvent += AddDamageOverTimeSkill;

        StartCoroutine(CastingActiveSkills());
    }

    private void OnDestroy()
    {
        CollisionHandler.characterHitEvent -= ApplyEffect;
        Bullet.characterHitEvent -= ApplyEffect;
        LevelingUI.addSkillEvent -= OnSkillAdded;
        DamageOverTimeSkill.addSkillEvent -= AddDamageOverTimeSkill;
    }
    #endregion

    private void OnSkillAdded(ISkill skill)
    {
        skill.AddSkill();

        // if (skill is IModifierSkill)
        // {
        //     IModifierSkill modifierSkill = skill as IModifierSkill;

        //     updatePlayerStat?.Invoke(modifierSkill.GetBonusStat());
        // }
        // else if (skill is DamageOverTimeSkill)
        // {
        //     damageSkills.Add((DamageOverTimeSkill)skill);
        // }
    }

    private void AddDamageOverTimeSkill(DamageOverTimeSkill skill)
    {
        damageSkills.Add(skill);
    }

    private void ApplyEffect(int instanceId)
    {
        // StatusEffectDamaging[] statusEffectDamagings = new StatusEffectDamaging[damageSkills.Count];

        // for (int i = 0; i < statusEffectDamagings.Length; i++)
        // {
        //     statusEffectDamagings[i] = damageSkills[i].StatusEffectDamaging;
        // }

        // applyDamagingStatusEffect?.Invoke(instanceId, statusEffectDamagings);

        statusEffectFromSkills = new List<IStatusEffect>();

        statusEffectFromSkills.Add(new StatusEffectCrowdControl());

        foreach (var statusEffect in statusEffectFromSkills)
        {
            statusEffect.ApplyStatusEffect(instanceId);
        }
    }

    private IEnumerator CastingActiveSkills()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);

        while (true)
        {
            if (activeSkillContainer.ActiveSkills != null)
            {
                foreach (var activeSkill in activeSkillContainer.ActiveSkills)
                {
                    if (activeSkill.IsUnlocked() && !activeSkill.IsInCountdown())
                    {
                        activeSkill.Cast();
                    }
                }
            }

            yield return waitForSeconds;
        }
    }
}
