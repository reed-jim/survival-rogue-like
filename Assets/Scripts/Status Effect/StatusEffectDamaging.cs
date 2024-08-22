using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/RPG/StatusEffectDamaging")]
public class StatusEffectDamaging : ScriptableObject, IDescription, ITier
{
    [SerializeField] private DamageOverTime damageOverTime;

    public float GetDamagePerSecond()
    {
        return damageOverTime.DamageEachTime / damageOverTime.DurationPerDamageInflicted;
    }

    public float GetTotalDamage()
    {
        return GetDamagePerSecond() * damageOverTime.Duration;
    }

    public string GetDescription()
    {
        return $"Deals total {GetTotalDamage()} every {damageOverTime.DurationPerDamageInflicted} damage over {damageOverTime.Duration} seconds.";
    }

    public int GetTier()
    {
        return 1;
    }
}
