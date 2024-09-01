using System.Collections;
using System.Collections.Generic;
using ReedJim.RPG.Stat;
using UnityEngine;

public class FlatStatModifier : IStatModifier
{
    public void ModifyValue(IStatComponent statComponent, float amount)
    {
        statComponent.Value += amount;
    }
}

public class MinusStatModifier : IStatModifier
{
    public void ModifyValue(IStatComponent statComponent, float amount)
    {
        statComponent.Value -= amount;
    }
}
