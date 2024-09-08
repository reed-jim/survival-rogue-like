using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkillContainer : MonoBehaviour
{
    [SerializeField] private IActiveSkill[] activeSkills;
    [SerializeField] private SkillContainer skillContainer;

    public IActiveSkill[] ActiveSkills => activeSkills;

    private void Awake()
    {
        GetAllActiveSkills();

        AddActiveSkillsToContainer();
    }

    private void GetAllActiveSkills()
    {
        activeSkills = new IActiveSkill[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            activeSkills[i] = transform.GetChild(i).GetComponent<IActiveSkill>();
        }
    }

    private void AddActiveSkillsToContainer()
    {
        skillContainer.AllSkills.Clear();

        skillContainer.AllSkills.AddRange(activeSkills);
    }
}
