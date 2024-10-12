using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Saferio.TreeBehaviour
{
    public class CharacterTreeBehaviour : MonoBehaviour
    {
        private INode _root;

        private SeekTargetNode _seekTargetNode;

        private void Awake()
        {
            _root = new Selector();

            Selector sequence = new Selector();

            _seekTargetNode = new SeekTargetNode();

            _seekTargetNode.SetInstanceId(gameObject.GetInstanceID());
            _seekTargetNode.RegisterEvent();

            sequence.AddChild(_seekTargetNode);
            sequence.AddChild(new AttackNode());

            ((Selector)_root).AddChild(sequence);
        }

        private void Update()
        {
            _root.Execute();
        }

        private void OnDestroy()
        {
            _seekTargetNode.UnregisterEvent();
        }
    }
}
