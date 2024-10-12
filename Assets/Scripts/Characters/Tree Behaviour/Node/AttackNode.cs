using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Saferio.TreeBehaviour
{
    public class AttackNode : INode
    {
        public bool Execute()
        {
            DebugUtil.DistinctLog("attack node");

            return true;
        }
    }
}
