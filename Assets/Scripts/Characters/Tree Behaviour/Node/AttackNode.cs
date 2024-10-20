using System.Collections;
using System.Collections.Generic;
using Saferio.Util.SaferioTween;
using UnityEngine;

namespace Saferio.TreeBehaviour
{
    public class AttackNode : INode
    {
        bool _fixLater = true;

        public bool Execute()
        {
            SaferioTween.Delay(2f, onCompletedAction: () => _fixLater = false);
            
            return _fixLater;
        }
    }
}
