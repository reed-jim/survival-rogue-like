using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleColorBlock : BaseBlock, IBlock
{
    private int _point;

    public int Point { get => _point; set => _point = value; }
    public int X { get; set; }
    public int Y { get; set; }

    public int GetNumberAdjacentBlock(List<IBlock> blocks)
    {
        int total = 0;

        foreach (var block in blocks)
        {
            if (Mathf.Abs(X - block.X) <= 1 && Mathf.Abs(Y - block.Y) <= 1)
            {
                total++;
            }
        }

        return total;
    }

    public void ShowPoint()
    {
        InvokeShowPointTextEvent(_point);
    }
}
