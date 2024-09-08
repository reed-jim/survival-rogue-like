using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using static CustomDelegate;

public class ActiveSkillMeteor : BaseActiveSkill, IActiveSkill
{
    [SerializeField] private GameObject player;

    #region ACTION
    public static event GetMeteorAction getMeteorAction;
    #endregion

    #region IActiveSkill Implement
    public void Cast()
    {
        Meteor meteor = getMeteorAction?.Invoke();

        meteor.gameObject.SetActive(true);

        meteor.transform.position = player.transform.position + new Vector3(Random.Range(5, 10), 10, Random.Range(5, 10));

        meteor.SetStat(Stat);

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
