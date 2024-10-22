using System;
using System.Collections;
using System.Collections.Generic;
using Saferio.Util.SaferioTween;
using UnityEngine;

namespace Saferio.TreeBehaviour
{
    public class AttackNode : INode
    {
        bool _fixLater = true;

        private int _instanceId;

        public static event Action<int> enableCharacterNavigationEvent;

        public AttackNode(int instanceId)
        {
            _instanceId = instanceId;
        }

        public bool Execute()
        {
            SaferioTween.Delay(2f, onCompletedAction: () =>
            {
                enableCharacterNavigationEvent?.Invoke(_instanceId);

                _fixLater = false;
            });

            return _fixLater;
        }
    }
}
