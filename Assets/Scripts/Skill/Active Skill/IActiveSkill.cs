using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActiveSkill
{
    public void Cast();
    public bool IsInCountdown();
}
