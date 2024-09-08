using System;
using System.Text;
using ReedJim.RPG.Stat;
using UnityEngine;

[CreateAssetMenu(fileName = "skill", menuName = "ScriptableObjects/RPG/DefenseSkill")]
public class DefenseSkill : ScriptableObject, IModifierSkill
{
    [SerializeField] private int hp;
    [SerializeField] private int armor;
    [SerializeField] private float blockChance;

    #region ACTION
    public static event Action<CharacterStat> updatePlayerStat;
    #endregion

    public CharacterStat GetBonusStat()
    {
        CharacterStat bonusStat = new CharacterStat();

        bonusStat.SetStatBaseValue(StatComponentNameConstant.Health, hp);
        bonusStat.SetStatBaseValue(StatComponentNameConstant.Armor, armor);
        bonusStat.SetStatBaseValue(StatComponentNameConstant.BlockChance, blockChance);

        return bonusStat;
    }

    #region ISkill Implement
    public string GetDescription()
    {
        StringBuilder description = new StringBuilder();

        if (hp > 0)
        {
            description.Append($"Increase {hp} HP. ");
        }

        if (armor > 0)
        {
            description.Append($"Increase {armor} Armor. ");
        }

        if (blockChance > 0)
        {
            description.Append($"Increase {blockChance * 100}% Block Chance.");
        }

        return description.ToString();
    }

    public string GetName()
    {
        return name;
    }

    public void AddSkill()
    {
        updatePlayerStat?.Invoke(GetBonusStat());
    }

    #endregion

    public int GetTier()
    {
        return 1;
    }
}
