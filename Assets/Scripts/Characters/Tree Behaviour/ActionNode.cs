using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionNode : INode
{
    private System.Func<bool> action;

    public ActionNode(System.Func<bool> action)
    {
        this.action = action;
    }

    public bool Execute()
    {
        return action.Invoke();
    }
}
