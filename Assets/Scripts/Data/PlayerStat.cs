using System;
using UnityEngine;

// [CreateAssetMenu(menuName = "ScriptableObjects/PlayerStat")]
[System.Serializable]
public class PlayerStat : CharacterStat
{
    public static event Action<float, float> updateExpProgressBarEvent;

    [SerializeField] private float exp;

    public float EXP
    {
        get => exp;
        set => exp = value;
    }

    private void Awake()
    {
        PlayerStat stat = DataUtility.Load<PlayerStat>(new PlayerStat());

        Debug.Log(stat.exp);
    }

    public void EarnExp(float earnedExp)
    {
        exp += earnedExp;

        if (exp > GetRequiredExpForNextLevel())
        {
            Level++;
            Damage += 10 * Level;

            exp = 0;

            // updateExpProgressBarEvent?.Invoke(exp, );
        }

        DataUtility.Save(this);
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