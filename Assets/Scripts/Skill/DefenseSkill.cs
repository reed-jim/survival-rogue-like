using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "skill", menuName = "ScriptableObjects/RPG/DefenseSkill")]
public class DefenseSkill : ScriptableObject, IModifierSkill
{
    [SerializeField] private int hp;
    [SerializeField] private int armor;
    [SerializeField] private int blockChance;

    public CharacterStat GetBonusStat()
    {
        return new CharacterStat()
        {
            HP = hp,
            Armor = armor,
            BlockChance = blockChance
        };
    }

    public string GetDescription()
    {
        StringBuilder description = new StringBuilder();

        if (hp > 0)
        {
            description.Append($"Increase {hp} HP. ");
        }

        if (armor > 0)
        {
            description.Append($"Increase {hp} Armor. ");
        }

        if (blockChance > 0)
        {
            description.Append($"Increase {hp * 100}% Block Chance.");
        }

        return description.ToString();
    }

    public int GetTier()
    {
        return 1;
    }
}
