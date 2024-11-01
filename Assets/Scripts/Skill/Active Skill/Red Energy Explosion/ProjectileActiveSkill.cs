using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CustomDelegate;

[CreateAssetMenu(menuName = "ScriptableObjects/RPG/ProjectileActiveSkill")]
public class ProjectileActiveSkill : BaseActiveSkill, IActiveSkill
{
    [SerializeField] private ActiveSkillIdentifer activeSkillIdentifer;
    // [SerializeField] private PlayerRuntime playerRuntime;

    #region PRIVATE FIELD
    private int _casterInstanceId;
    #endregion

    #region ACTION
    public static event Action<ActiveSkillIdentifer> activateActiveSkillEvent;
    #endregion

    #region IActiveSkill Implement

    public override bool IsUnlocked()
    {
        return IsSkillUnlocked;
    }

    public override bool IsInCountdown()
    {
        return IsCountdown;
    }
    #endregion

    public override void AddSkill()
    {
        // InvokeAddActiveSkillEvent(_casterInstanceId);
        activateActiveSkillEvent?.Invoke(activeSkillIdentifer);

        IsCountdown = false;

        IsSkillUnlocked = true;
    }

    public override string GetDescription()
    {
        string description = $"{name}\n";

        string rarityTierColor = SurvivoriumTheme.RARITY_COLORs[_rarityTier];

        foreach (var statComponent in Stat.StatComponents)
        {
            if (statComponent.Value.BaseValue > 0)
            {
                description += $"<color=#fff>{statComponent.Key} - <color={rarityTierColor}>{statComponent.Value.BaseValue}</color>\n";
            }
        }

        return description;
    }

    public override string GetName()
    {
        return name;
    }

    public override int GetTier()
    {
        _rarityTier = UnityEngine.Random.Range(0, 5);

        return _rarityTier;
    }
}
