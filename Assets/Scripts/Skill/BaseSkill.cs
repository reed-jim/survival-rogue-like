using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkill : ScriptableObject, ISkill
{
    public virtual void AddSkill()
    {
        throw new System.NotImplementedException();
    }

    public virtual string GetDescription()
    {
        throw new System.NotImplementedException();
    }

    public virtual string GetName()
    {
        throw new System.NotImplementedException();
    }

    public virtual int GetTier()
    {
        return Random.Range(0, 5);
    }
}
