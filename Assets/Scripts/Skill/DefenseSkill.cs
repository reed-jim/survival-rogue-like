using System;
using System.Text;
using ReedJim.RPG.Stat;
using UnityEngine;

[CreateAssetMenu(fileName = "skill", menuName = "ScriptableObjects/RPG/DefenseSkill")]
public class DefenseSkill : BaseSkill, IModifierSkill
{
    [SerializeField] private int hp;
    [SerializeField] private int armor;
    [SerializeField] private float blockChance;

    private int _currentTier = -1;

    #region ACTION
    public static event Action<CharacterStat> updatePlayerStat;
    #endregion

    public CharacterStat GetBonusStat()
    {
        CharacterStat bonusStat = new CharacterStat();

        bonusStat.SetStatBaseValue(StatComponentNameConstant.Health, hp * (_currentTier + 1));
        bonusStat.SetStatBaseValue(StatComponentNameConstant.Armor, armor * (_currentTier + 1));
        bonusStat.SetStatBaseValue(StatComponentNameConstant.BlockChance, blockChance * (_currentTier + 1));

        return bonusStat;
    }

    #region ISkill Implement
    public override string GetDescription()
    {
        StringBuilder description = new StringBuilder();

        string colorStringByTier = SurvivoriumTheme.RARITY_COLORs[_currentTier];

        if (hp > 0)
        {
            description.Append($"Increase <color={colorStringByTier}>{hp * (_currentTier + 1)}</color> HP. ");
        }

        if (armor > 0)
        {
            description.Append($"Increase <color={colorStringByTier}>{armor * (_currentTier + 1)}</color> Armor. ");
        }

        if (blockChance > 0)
        {
            description.Append($"Increase <color={colorStringByTier}>{blockChance * 100 * (_currentTier + 1)}%</color> Block Chance.");
        }

        return description.ToString();
    }

    public override string GetName()
    {
        return name;
    }

    public override void AddSkill()
    {
        updatePlayerStat?.Invoke(GetBonusStat());
    }

    #endregion

    public override int GetTier()
    {
        if (_currentTier == -1)
        {
            _currentTier = UnityEngine.Random.Range(0, 5);
        }

        return _currentTier;
    }
}
