using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class Selector : IComposite
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
            if (child.Execute())
            {
                return true;
            }
        }

        return false;
    }
}
