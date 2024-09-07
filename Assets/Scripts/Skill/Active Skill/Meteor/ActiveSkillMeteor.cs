using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class ActiveSkillMeteor : BaseActiveSkill, IActiveSkill
{
    [SerializeField] private GameObject meteor;

    public void Cast()
    {
        meteor.gameObject.SetActive(true);

        meteor.transform.position = new Vector3(0, 10, 0);

        IsCountdown = true;

        Tween.Delay(Stat.GetStatValue(StatComponentNameConstant.AttackSpeed))
            .OnComplete(() => IsCountdown = false);
    }

    public bool IsInCountdown()
    {
        return IsCountdown;
    }
}
