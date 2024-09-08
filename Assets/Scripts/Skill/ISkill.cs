using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill : ITier
{
    public string GetDescription();
    public string GetName();
    public void AddSkill();
}
