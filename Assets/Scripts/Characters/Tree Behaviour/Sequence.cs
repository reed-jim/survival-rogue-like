using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Saferio.TreeBehaviour
{
    public class Sequence : IComposite
    {
        public List<INode> children = new List<INode>();

        public void AddChild(INode child)
        {
            children.Add(child);
        }

        public bool Execute()
        {
            foreach (var child in children)
            {
                if (!child.Execute())
                {
                    return false;
                }
            }

            return true;
        }
    }
}
