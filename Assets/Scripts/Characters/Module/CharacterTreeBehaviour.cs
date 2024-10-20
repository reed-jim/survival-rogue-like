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

            Selector selector = new Selector();

            _seekTargetNode = new SeekTargetNode();

            _seekTargetNode.SetInstanceId(gameObject.GetInstanceID());
            _seekTargetNode.RegisterEvent();

            selector.AddChild(_seekTargetNode);
            selector.AddChild(new AttackNode());

            ((Selector)_root).AddChild(selector);
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
