using UnityEngine;

[CreateAssetMenu(fileName = "skill", menuName = "ScriptableObjects/RPG/DamageSkill")]
public class DamageOverTimeSkill : ScriptableObject, ISkill
{
    [SerializeField] private float totalDamage;
    [SerializeField] private float duration;

    public float TotalDamage
    {
        set => totalDamage = value;
    }

    public float Duration
    {
        set => duration = value;
    }

    public float DamagePerSecond => totalDamage / duration;

    public string GetDescription()
    {
        return $"Trigger Burn effect. Burn: Cause {totalDamage} over {duration}s";
    }

    public int GetTier()
    {
        return Random.Range(1, 5);
    }
}
