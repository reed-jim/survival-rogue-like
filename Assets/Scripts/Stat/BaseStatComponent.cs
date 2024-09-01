using System;
using System.Collections;
using System.Collections.Generic;
using ReedJim.RPG.Stat;
using UnityEngine;

namespace ReedJim.RPG.Stat
{
    [Serializable]
    public class BaseStatComponent : IStatComponent
    {
        [SerializeField] private float baseValue;

        public float Value { get; set; }
        public float BaseValue { get => baseValue; set => baseValue = value; }

        public void Reset()
        {
            Value = BaseValue;
        }

        // public virtual void ModifyValue(IStatModifier statModifier, float amount)
        // {
        //     statModifier.ModifyValue(Value, amount);
        // }
    }
}
