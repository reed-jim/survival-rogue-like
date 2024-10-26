using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Saferio.TreeBehaviour
{
    public class IsDeadNode : INode
    {
        private bool _isDead;
        private int _instanceId;

        public IsDeadNode(int instanceId)
        {
            _instanceId = instanceId;
        }

        public void RegisterEvent()
        {
            CharacterStatManager.characterDieEvent += SetIsDead;
        }

        public void UnregisterEvent()
        {
            CharacterStatManager.characterDieEvent -= SetIsDead;
        }

        public bool Execute()
        {
            return _isDead;
        }

        private void SetIsDead(int instanceId)
        {
            if (instanceId == _instanceId)
            {

            }
        }
    }
}
