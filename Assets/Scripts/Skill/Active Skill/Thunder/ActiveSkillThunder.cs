using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class ActiveSkillThunder : BaseActiveSkill, IActiveSkill
{
    [SerializeField] private Transform player;
    [SerializeField] private ChainLighning chainLighning;

    public void Cast()
    {
        chainLighning.gameObject.SetActive(true);

        chainLighning.transform.position = player.position;

        IsCountdown = true;

        Tween.Delay(Stat.GetStatValue(StatComponentNameConstant.AttackSpeed))
            .OnComplete(() => IsCountdown = false);
    }

    public bool IsInCountdown()
    {
        return IsCountdown;
    }
}
