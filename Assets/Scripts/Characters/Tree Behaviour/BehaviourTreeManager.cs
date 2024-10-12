using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Saferio.TreeBehaviour
{
    public class BehaviourTreeManager : MonoBehaviour
    {
        private INode _root;

        private void Start()
        {
            _root = new Selector();

            Sequence sequence = new Sequence();

            sequence.AddChild(new ActionNode(IsTargetFound));
            sequence.AddChild(new ActionNode(Patrolling));
        }

        private bool IsTargetFound()
        {
            return true;
        }

        private bool Patrolling()
        {
            return true;
        }
    }
}
