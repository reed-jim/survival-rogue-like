using System;
using System.Collections;
using System.Collections.Generic;
using ReedJim.RPG.Stat;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterSkill : MonoBehaviour
{
    // [SerializeField] private ActiveSkillContainer activeSkillContainer;
    [SerializeField] private List<DamageOverTimeSkill> damageSkills;
    [SerializeField] private List<IStatusEffect> statusEffects;
    [SerializeField] private List<IActiveSkill> activeSkills;

    #region PRIVATE FIELD
    private Transform _lastNearestEnemy;
    #endregion

    #region ACTION
    public static event Action<int, StatusEffectDamaging[]> applyDamagingStatusEffect;
    public static event Action<CharacterStat> updatePlayerStat;
    #endregion

    #region LIFE CYCLE
    private void Awake()
    {
        CollisionHandler.characterHitEvent += ApplyEffect;
        Bullet.characterHitEvent += ApplyEffect;
        SkillSelectionSlot.addSkillEvent += OnSkillAdded;
        DamageOverTimeSkill.addSkillEvent += AddDamageOverTimeSkill;
        ModifedSkillWithStatusEffect.addStatusEffectToSkillEvent += AddStatusEffectAbility;
        CharacterVision.attackEnemyEvent += OnEnemyFound;
        BaseActiveSkill.addActiveSkillEvent += AddActiveSkill;

        damageSkills = new List<DamageOverTimeSkill>();
        statusEffects = new List<IStatusEffect>();
        activeSkills = new List<IActiveSkill>();

        StartCoroutine(CastingActiveSkills());
    }

    private void OnDestroy()
    {
        CollisionHandler.characterHitEvent -= ApplyEffect;
        Bullet.characterHitEvent -= ApplyEffect;
        SkillSelectionSlot.addSkillEvent -= OnSkillAdded;
        DamageOverTimeSkill.addSkillEvent -= AddDamageOverTimeSkill;
        ModifedSkillWithStatusEffect.addStatusEffectToSkillEvent -= AddStatusEffectAbility;
        CharacterVision.attackEnemyEvent -= OnEnemyFound;
        BaseActiveSkill.addActiveSkillEvent -= AddActiveSkill;
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

    private void AddStatusEffectAbility(IStatusEffect statusEffect)
    {
        statusEffects.Add(statusEffect);
    }

    private void ApplyEffect(int instanceId)
    {
        // StatusEffectDamaging[] statusEffectDamagings = new StatusEffectDamaging[damageSkills.Count];

        // for (int i = 0; i < statusEffectDamagings.Length; i++)
        // {
        //     statusEffectDamagings[i] = damageSkills[i].StatusEffectDamaging;
        // }

        // applyDamagingStatusEffect?.Invoke(instanceId, statusEffectDamagings);

        foreach (var statusEffect in statusEffects)
        {
            statusEffect.ApplyStatusEffect(instanceId);
        }
    }

    private void AddActiveSkill(int instanceId, IActiveSkill activeSkill)
    {
        activeSkills.Add(activeSkill);
    }

    private IEnumerator CastingActiveSkills()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);

        while (true)
        {
            if (activeSkills != null)
            {
                foreach (var activeSkill in activeSkills)
                {
                    if (!activeSkill.IsInCountdown())
                    {
                        activeSkill.Cast();
                    }
                }
            }

            yield return waitForSeconds;
        }
    }

    private void OnEnemyFound(int instanceId, Transform enemy)
    {
        if (instanceId == gameObject.GetInstanceID())
        {
            _lastNearestEnemy = enemy;
        }
    }
}
