using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Saferio.TreeBehaviour
{
    public class CharacterTreeBehaviour : MonoBehaviour
    {
        private INode _root;

        private IsDeadNode _isDeadNode;
        private SeekTargetNode _seekTargetNode;

        private void Awake()
        {
            _root = new Selector();

            Selector selector = new Selector();

            int instanceId = gameObject.GetInstanceID();

            _isDeadNode = new IsDeadNode(instanceId);
            _isDeadNode.RegisterEvent();

            _seekTargetNode = new SeekTargetNode(instanceId);
            _seekTargetNode.RegisterEvent();

            selector.AddChild(_isDeadNode);

            Sequence sequenceNode = new Sequence();

            sequenceNode.AddChild(_seekTargetNode);
            sequenceNode.AddChild(new AttackNode(instanceId));

            ((Selector)_root).AddChild(selector);
            ((Selector)_root).AddChild(sequenceNode);
        }

        private void Update()
        {
            _root.Execute();
        }

        private void OnDestroy()
        {
            _isDeadNode.UnregisterEvent();
            _seekTargetNode.UnregisterEvent();
        }
    }
}
