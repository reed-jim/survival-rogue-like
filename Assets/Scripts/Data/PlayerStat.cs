using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerStat")]
public class PlayerStat : CharacterStat
{
    public static event Action<float> updateExpProgressBarEvent;

    [SerializeField] private float exp;

    public float EXP
    {
        get => exp;
        set => exp = value;
    }

    public void EarnExp(float earnedExp)
    {
        exp += earnedExp;

        if (exp > GetRequiredExpForNextLevel())
        {
            Level++;
            Damage += 10 * Level;

            exp = 0;

            updateExpProgressBarEvent?.Invoke(exp);
        }
    }

    public float GetExpFromKillEnemy(int enemyLevel)
    {
        return 50f / (Level - enemyLevel + 1) + 5 * (Level - 1);
    }

    public float GetRequiredExpForNextLevel()
    {
        return 100 * Level + 5 * Level * (Level - 1);
    }
}
