using System;
using System.Collections;
using System.Collections.Generic;
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
                CharacterStat damageStat = new CharacterStat()
                {
                    Damage = damagingStatusEffect.GetDamagePerSecond(),
                    DamageMultiplier = 1,
                    PercentDirectDamage = 0
                };
                
                applyDamageEvent?.Invoke(gameObject.GetInstanceID(), damageStat);
            }

            yield return waitForSeconds;
        }
    }
}
