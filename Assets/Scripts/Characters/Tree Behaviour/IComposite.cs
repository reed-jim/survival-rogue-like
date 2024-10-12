using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IComposite : INode
{
    public void AddChild(INode child);
}
