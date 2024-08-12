using System;
using UnityEngine;

[System.Serializable]
public class PlayerStat : CharacterStat
{
    public static event Action<float, float> updateExpProgressBarEvent;

    [SerializeField] private float exp;

    [SerializeField] private float reloadTime = 1;

    public float EXP
    {
        get => exp;
        set => exp = value;
    }

    public float ReloadTime
    {
        get => reloadTime;
        set => reloadTime = value;
    }

    private void Awake()
    {
        PlayerStat stat = DataUtility.Load<PlayerStat>(new PlayerStat());
    }

    public void EarnExp(float earnedExp, out bool isLeveledUp)
    {
        isLeveledUp = false;

        exp += earnedExp;

        if (exp > GetRequiredExpForNextLevel())
        {
            Level++;

            exp = 0;

            isLeveledUp = true;

            // updateExpProgressBarEvent?.Invoke(exp, );
        }

        DataUtility.Save(this);
    }

    public float GetMaxHp()
    {
        return 100;
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
