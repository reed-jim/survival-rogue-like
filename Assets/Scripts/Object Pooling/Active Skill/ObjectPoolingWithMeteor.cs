using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingWithMeteor : ObjectPoolingWithOneComponent<Meteor>
{
    protected override void Awake()
    {
        base.Awake();

        ActiveSkillMeteor.getMeteorAction += GetComponentFromPool;
    }

    private void OnDestroy()
    {
        ActiveSkillMeteor.getMeteorAction -= GetComponentFromPool;
    }
}
