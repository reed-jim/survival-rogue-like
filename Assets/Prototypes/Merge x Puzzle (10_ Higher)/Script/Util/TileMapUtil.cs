using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileMapUtil
{
    public static Vector3 GetPositionFromCoordinator(Vector2 coordinator, Vector3 firstTilePosition, Vector3 tileDistance)
    {
        return new Vector3(firstTilePosition.x + coordinator.x * tileDistance.x, firstTilePosition.y, firstTilePosition.z + coordinator.y * tileDistance.z);
    }
}
