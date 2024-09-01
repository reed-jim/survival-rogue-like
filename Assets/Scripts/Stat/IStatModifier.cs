using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReedJim.RPG.Stat
{
    public interface IStatModifier
    {
        public void ModifyValue(IStatComponent statComponent, float amount);
    }
}
