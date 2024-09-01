using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReedJim.RPG.Stat
{
    public interface IStatComponent
    {
        public float Value { get; set; }
        public float BaseValue { get; set; }
        public void Reset();

        // public void ModifyValue(IStatModifier statModifier, float amount);
    }
}
