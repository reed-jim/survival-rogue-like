using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Saferio.TreeBehaviour
{
    public class SeekTargetNode : INode
    {
        private int _instanceId;

        private bool _isBehaviourInProgress = false;
        private bool _isSeekingTarget = true;

        public static event Action startSeekTargetEvent;
        public static event Action<int> startSeekTargetBahaviourEvent;

        public void SetInstanceId(int instanceId)
        {
            _instanceId = instanceId;
        }

        public void RegisterEvent()
        {
            CharacterNavigation.targetFoundEvent += SetIsSeekingTarget;
        }

        public void UnregisterEvent()
        {
            CharacterNavigation.targetFoundEvent -= SetIsSeekingTarget;
        }

        public bool Execute()
        {
            if (!_isBehaviourInProgress)
            {
                startSeekTargetBahaviourEvent?.Invoke(_instanceId);

                _isBehaviourInProgress = true;
            }

            return _isSeekingTarget;
        }

        private void SetIsSeekingTarget()
        {
            _isSeekingTarget = false;
        }
    }
}
