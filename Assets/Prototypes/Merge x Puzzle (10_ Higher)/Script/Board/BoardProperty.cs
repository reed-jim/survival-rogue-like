using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Board Property", menuName = "ScriptableObjects/Prototype/BoardProperty")]
public class BoardProperty : ScriptableObject
{
    [SerializeField] private int maxRow;
    [SerializeField] private int maxColumn;

    public int MaxRow
    {
        get => maxRow; set => maxRow = value;
    }

    public int MaxColumn
    {
        get => maxColumn; set => maxColumn = value;
    }

    public int MaxTile => maxRow * maxColumn;
    public Vector2 TileSize { get; set; }
    public Vector3 TileDistance { get; set; }
    public Vector3 FirstTilePosition { get; set; }
}
