using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandTile : BaseTile, ITile
{
    public int X { get; set; }
    public int Y { get; set; }
    public Vector3 Position { get; set; }
    public int TileRichness { get; set; }
}
