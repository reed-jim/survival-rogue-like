using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class ActiveSkillThunder : BaseActiveSkill, IActiveSkill
{
    [SerializeField] private Transform player;
    [SerializeField] private ChainLighning chainLighning;

    #region IActiveSkill Implement
    public void Cast()
    {
        chainLighning.gameObject.SetActive(true);

        chainLighning.transform.position = player.position;

        IsCountdown = true;

        Tween.Delay(Stat.GetStatValue(StatComponentNameConstant.AttackSpeed))
            .OnComplete(() => IsCountdown = false);
    }

    public bool IsUnlocked()
    {
        return IsSkillUnlocked;
    }

    public bool IsInCountdown()
    {
        return IsCountdown;
    }
    #endregion

    public void AddSkill()
    {
        IsSkillUnlocked = true;
    }

    public string GetDescription()
    {
        return "Lightning Chain";
    }

    public string GetName()
    {
        return "Lightning Chain";
    }

    public int GetTier()
    {
        return 0;
    }
}
