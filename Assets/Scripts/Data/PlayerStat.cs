using System;
using ReedJim.RPG.Stat;
using UnityEngine;

[Serializable]
public class PlayerStat : CharacterStat
{
    public static event Action<float, float> updateExpProgressBarEvent;

    public PlayerStat(CharacterStat characterStat)
    {
        statComponents = characterStat.StatComponents;
    }

    public void EarnExp(float earnedExp, out bool isLeveledUp)
    {
        isLeveledUp = false;

        ModifyStat(StatComponentNameConstant.Experience, new FlatStatModifier(), earnedExp);

        if (GetStatValue(StatComponentNameConstant.Experience) > GetRequiredExpForNextLevel())
        {
            ModifyStat(StatComponentNameConstant.Level, new FlatStatModifier(), 1);

            SetZero(StatComponentNameConstant.Experience);

            // ResetStat(StatComponentNameConstant.Experience);

            isLeveledUp = true;
        }

        DataUtility.Save(Constants.STAT_DATA_FILE_NAME, Constants.PLAYER_TAG, this);
    }

    public float GetMaxHp()
    {
        return 100;
    }

    public float GetExpFromKillEnemy(int enemyLevel)
    {
        return 15 * (GetStatValue(StatComponentNameConstant.Level) + enemyLevel - 1);
    }

    public float GetRequiredExpForNextLevel()
    {
        int level = (int)GetStatValue(StatComponentNameConstant.Level);

        return 100 * level + 200 * level * (level - 1);
    }

    public static PlayerStat operator +(PlayerStat currentStat, CharacterStat bonusStat)
    {
        PlayerStat newStat = currentStat;

        FlatStatModifier flatStatModifier = new FlatStatModifier();

        foreach (var key in newStat.GetStatKeys())
        {
            if (bonusStat.StatComponents.ContainsKey(key))
            {
                newStat.ModifyStat(key, flatStatModifier, bonusStat.GetStatBaseValue(key));
            }
        }

        return newStat;
    }
}
