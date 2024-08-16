using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomSkillMachine
{
    public static ISkill GetRandomSkill(SkillContainer skillContainer)
    {
        return skillContainer.AllSkills[Random.Range(0, skillContainer.NumberOfSkill)];
    }
}
