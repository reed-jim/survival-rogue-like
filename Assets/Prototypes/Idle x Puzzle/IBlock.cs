using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlock
{
    public int Point
    {
        get; set;
    }

    public int X
    {
        get; set;
    }

    public int Y
    {
        get; set;
    }

    public void ShowPoint();
    public int GetNumberAdjacentBlock(List<IBlock> blocks);
}
