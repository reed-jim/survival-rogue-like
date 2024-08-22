using System;
using UnityEngine;

[Serializable]
public class DamageOverTime
{
    [SerializeField] private float damageEachTime;
    [SerializeField] private float duration;
    [SerializeField] private float durationPerDamageInflicted;

    public float DamageEachTime => damageEachTime;
    public float Duration => duration;
    public float DurationPerDamageInflicted => durationPerDamageInflicted;

    // private IEnumerator CauseDamageOverTime()
    // {
    //     WaitForSeconds waitForSeconds = new WaitForSeconds(durationPerDamageInflicted);

    //     float timePassed = 0;

    //     while (timePassed < duration)
    //     {
    //         // cause damage

    //         yield return waitForSeconds;
    //     }
    // }
}