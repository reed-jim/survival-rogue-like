using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;

    #region PRIVATE FIELD
    private List<IBlock> _blocks;
    #endregion

    #region ACTION
    public static event Action<IBlock> addBlockEvent;
    #endregion

    private void Awake()
    {
        _blocks = new List<IBlock>();

        InputManager.spawnBlockEvent += Build;
    }

    private void OnDestroy()
    {
        InputManager.spawnBlockEvent -= Build;
    }

    private void Build(ITile tileComponent)
    {
        GameObject block = Instantiate(blockPrefab);

        Vector3 spawnPosition = tileComponent.Position;

        spawnPosition.y = tileComponent.Position.y + 3;

        block.transform.position = spawnPosition;

        IBlock blockComponent = block.GetComponent<IBlock>();

        blockComponent.X = tileComponent.X;
        blockComponent.Y = tileComponent.Y;
        blockComponent.Point = (blockComponent.GetNumberAdjacentBlock(_blocks) + 1) * tileComponent.TileRichness;

        addBlockEvent?.Invoke(blockComponent);

        _blocks.Add(blockComponent);
    }
}
