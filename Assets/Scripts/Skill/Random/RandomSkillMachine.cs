using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomSkillMachine
{
    public static ISkill GetRandomSkill(SkillContainer skillContainer)
    {
        Debug.Log(skillContainer.NumberOfSkill);
        return skillContainer.AllSkills[Random.Range(0, skillContainer.NumberOfSkill)];
    }

    public static ISkill[] GetThreeRandomSkills(SkillContainer skillContainer)
    {
        ISkill[] randomSkills = new ISkill[3];

        SkillContainer avoidDuplicateSkillContainer = new SkillContainer();

        avoidDuplicateSkillContainer.AllSkills.AddRange(skillContainer.AllSkills);

        for (int i = 0; i < randomSkills.Length; i++)
        {
            randomSkills[i] = GetRandomSkill(avoidDuplicateSkillContainer);

            avoidDuplicateSkillContainer.AllSkills.Remove((BaseSkill)randomSkills[i]);
        }

        return randomSkills;
    }
}
