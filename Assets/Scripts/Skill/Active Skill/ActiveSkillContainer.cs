using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActiveSkillContainer : MonoBehaviour
{
    [SerializeField] private BaseActiveSkill[] activeSkills;
    [SerializeField] private SkillContainer skillContainer;

    public BaseActiveSkill[] ActiveSkills => activeSkills;

    private void Awake()
    {
        AddActiveSkillsToContainer();
    }

    private void AddActiveSkillsToContainer()
    {
        skillContainer.AllSkills.AddRange(activeSkills.ToArray());
    }
}
