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

    public Vector3 TileSize { get; set; }
    public Vector3 TileDistance { get; set; }
    public Vector3 FirstTilePosition { get; set; }

    public int MaxTile => maxRow * maxColumn;
    public Vector3 BoardSize
    {
        get
        {
            return new Vector3(
                (MaxColumn * TileDistance.x) + TileSize.x,
                TileSize.y,
                (MaxRow * TileDistance.z) + TileSize.z
            );
        }
    }
    public float MinPositionX => FirstTilePosition.x;
    public float MaxPositionX => FirstTilePosition.x - 0.5f * TileSize.x + BoardSize.x;
    public float MinPositionZ => FirstTilePosition.z;
    public float MaxPositionZ => FirstTilePosition.z - 0.5f * TileSize.z + BoardSize.z;
}
