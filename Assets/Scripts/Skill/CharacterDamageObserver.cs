using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDamageObserver : MonoBehaviour
{
    public static event Action<int, float> applyDamageEvent;

    private List<DamageOverTimeSkill> _damageSkillsTaken;

    #region PRIVATE FIELD
    private Coroutine _takingDamageCoroutine;
    #endregion

    private void Awake()
    {
        _damageSkillsTaken = new List<DamageOverTimeSkill>();

        CharacterSkill.applyDamageEvent += ApplyDamage;
    }

    private void OnDestroy()
    {
        CharacterSkill.applyDamageEvent -= ApplyDamage;
    }

    private void ApplyDamage(string instanceId, DamageOverTimeSkill damageOverTimeSkill)
    {
        if (gameObject.GetInstanceID().ToString() == instanceId)
        {
            _damageSkillsTaken.Add(damageOverTimeSkill);

            if (_takingDamageCoroutine == null)
            {
                _takingDamageCoroutine = StartCoroutine(TakingDamage());
            }
        }
    }

    private IEnumerator TakingDamage()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1f);

        while (true)
        {
            if (_damageSkillsTaken.Count > 0)
            {
                applyDamageEvent?.Invoke(gameObject.GetInstanceID(), _damageSkillsTaken[0].DamagePerSecond);
            }

            yield return waitForSeconds;
        }
    }
}
