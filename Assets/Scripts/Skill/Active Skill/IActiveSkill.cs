using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActiveSkill : ISkill
{
    public void Cast();
    public bool IsUnlocked();
    public bool IsInCountdown();
}
