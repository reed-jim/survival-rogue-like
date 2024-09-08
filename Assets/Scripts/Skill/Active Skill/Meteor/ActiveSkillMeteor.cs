using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using static CustomDelegate;

public class ActiveSkillMeteor : BaseActiveSkill, IActiveSkill
{
    [SerializeField] private GameObject meteor;

    #region ACTION
    public static event GetMeteorAction getMeteorAction;
    #endregion

    #region IActiveSkill Implement
    public void Cast()
    {
        meteor = getMeteorAction?.Invoke().gameObject;

        meteor.gameObject.SetActive(true);

        meteor.transform.position = new Vector3(0, 10, 0);

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

    public string GetDescription()
    {
        return "Meteor";
    }

    public string GetName()
    {
        return "Meteor";
    }

    public int GetTier()
    {
        throw new System.NotImplementedException();
    }

    public void AddSkill()
    {
        IsSkillUnlocked = true;
    }
}
