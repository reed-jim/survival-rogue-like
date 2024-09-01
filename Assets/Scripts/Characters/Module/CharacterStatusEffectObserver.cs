using System;
using System.Collections;
using System.Collections.Generic;
using ReedJim.RPG.Stat;
using UnityEngine;

public class CharacterStatusEffectObserver : MonoBehaviour
{
    private List<IModifierSkill> modifierStatusEffects;
    private List<StatusEffectDamaging> damagingStatusEffects;

    #region ACTION
    public static event Action<int, CharacterStat> applyDamageEvent;
    #endregion

    private void Awake()
    {
        damagingStatusEffects = new List<StatusEffectDamaging>();

        CharacterSkill.applyDamagingStatusEffect += OnDamagingStatusEffectAdded;
    }

    private void OnEnable()
    {
        StartCoroutine(ObservingStatusEffect());
    }

    private void OnDestroy()
    {
        CharacterSkill.applyDamagingStatusEffect -= OnDamagingStatusEffectAdded;
    }

    private void OnDamagingStatusEffectAdded(int instanceId, StatusEffectDamaging[] statusEffectDamagings)
    {
        if (instanceId == gameObject.GetInstanceID())
        {
            damagingStatusEffects.AddRange(statusEffectDamagings);
        }
    }

    private IEnumerator ObservingStatusEffect()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1f);

        while (true)
        {
            foreach (var damagingStatusEffect in damagingStatusEffects)
            {
                CharacterStat damageStat = new CharacterStat();

                damageStat.SetStatBaseValue(StatComponentNameConstant.Damage, damagingStatusEffect.GetDamagePerSecond());

                applyDamageEvent?.Invoke(gameObject.GetInstanceID(), damageStat);
            }

            yield return waitForSeconds;
        }
    }
}
