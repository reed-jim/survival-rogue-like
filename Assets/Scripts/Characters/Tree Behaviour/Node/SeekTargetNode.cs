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
        private bool _isTargetFound;

        public static event Action startSeekTargetEvent;
        public static event Action<int> startSeekTargetBahaviourEvent;

        public SeekTargetNode(int instanceId)
        {
            _instanceId = instanceId;
        }

        public void SetInstanceId(int instanceId)
        {
            _instanceId = instanceId;
        }

        public void RegisterEvent()
        {
            CharacterNavigation.targetFoundEvent += StopSeekingTarget;
            CharacterStatManager.characterDieEvent += StopSeekingTarget;
            AttackNode.enableCharacterNavigationEvent += EnableSeekingTarget;
        }

        public void UnregisterEvent()
        {
            CharacterNavigation.targetFoundEvent -= StopSeekingTarget;
            CharacterStatManager.characterDieEvent -= StopSeekingTarget;
            AttackNode.enableCharacterNavigationEvent -= EnableSeekingTarget;
        }

        public bool Execute()
        {
            if (!_isBehaviourInProgress)
            {
                startSeekTargetBahaviourEvent?.Invoke(_instanceId);

                _isBehaviourInProgress = true;
            }

            return _isTargetFound;
        }

        // private void SetIsSeekingTarget(int instanceId)
        // {
        //     if (instanceId == _instanceId)
        //     {
        //         _isSeekingTarget = false;
        //     }
        // }

        private void StopSeekingTarget(int instanceId)
        {
            if (instanceId == _instanceId)
            {
                _isTargetFound = true;
            }
        }

        private void EnableSeekingTarget(int instanceId)
        {
            if (instanceId == _instanceId)
            {
                startSeekTargetBahaviourEvent?.Invoke(_instanceId);

                _isTargetFound = false;
            }
        }
    }
}
