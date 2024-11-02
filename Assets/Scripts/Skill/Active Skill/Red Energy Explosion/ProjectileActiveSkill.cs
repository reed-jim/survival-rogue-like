using System;
using System.Collections;
using System.Collections.Generic;
using ReedJim.RPG.Stat;
using Saferio.Util.SaferioTween;
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
    public static event Action<ActiveSkillIdentifer, CharacterStat> activateActiveSkillEvent;
    public static event Action<ActiveSkillIdentifer> castActiveSkillEvent;
    #endregion

    #region IActiveSkill Implement
    public override void Cast()
    {
        castActiveSkillEvent?.Invoke(activeSkillIdentifer);

        IsCountdown = true;

        SaferioTween.Delay(2f, onCompletedAction:() => IsCountdown = false);
    }

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
        InvokeAddActiveSkillEvent(_casterInstanceId);
        activateActiveSkillEvent?.Invoke(activeSkillIdentifer, Stat);

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
