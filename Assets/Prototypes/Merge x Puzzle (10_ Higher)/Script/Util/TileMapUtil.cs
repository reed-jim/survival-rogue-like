using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileMapUtil
{
    public static Vector3 GetPositionFromCoordinator(Vector2 coordinator, Vector3 firstTilePosition, Vector3 tileDistance)
    {
        return new Vector3(firstTilePosition.x + coordinator.x * tileDistance.x, firstTilePosition.y, firstTilePosition.z + coordinator.y * tileDistance.z);
    }

    public static TileLocation GetSnapPosition(Vector3 mousePosition, BoardProperty boardProperty)
    {
        TileLocation snapLocation = new TileLocation();

        Vector2Int snapCoordinator = new Vector2Int();
        Vector3 snapPosition = new Vector3();

        if (mousePosition.x > boardProperty.MinPositionX && mousePosition.x < boardProperty.MaxPositionX)
        {
            if (mousePosition.z < boardProperty.MinPositionZ)
            {
                snapCoordinator.y = 0;
                snapPosition.z = boardProperty.MinPositionZ - 2;
            }
            else if (mousePosition.z > boardProperty.MaxPositionZ)
            {
                snapCoordinator.y = boardProperty.MaxRow;
                snapPosition.z = boardProperty.MaxPositionZ + 2;
            }

            int snapCoordinatorX;

            snapPosition.x = GetSnapPositionX(mousePosition.x, boardProperty, out snapCoordinatorX);

            snapCoordinator.x = snapCoordinatorX;
        }
        else
        {
            if (mousePosition.x < boardProperty.MinPositionX)
            {
                snapCoordinator.x = 0;
                snapPosition.x = boardProperty.MinPositionX - 2;
            }
            else if (mousePosition.x > boardProperty.MaxPositionX)
            {
                snapCoordinator.x = boardProperty.MaxColumn;
                snapPosition.x = boardProperty.MaxPositionX + 2;
            }

            int snapCoordinatorY;

            snapPosition.z = GetSnapPositionZ(mousePosition.z, boardProperty, out snapCoordinatorY);

            snapCoordinator.y = snapCoordinatorY;
        }

        snapLocation.Coordinator = snapCoordinator;
        snapLocation.Position = snapPosition;

        return snapLocation;
    }

    private static float GetSnapPositionX(float mousePositionX, BoardProperty boardProperty, out int snapCoordinatorX)
    {
        snapCoordinatorX = 0;
        float snapPositionX = 0;
        float min = float.MaxValue;

        for (int i = 0; i < boardProperty.MaxColumn; i++)
        {
            float tilePosition = boardProperty.FirstTilePosition.x + i * boardProperty.TileDistance.x;
            float distance = Mathf.Abs(mousePositionX - tilePosition);

            if (distance < min)
            {
                snapPositionX = tilePosition;
                min = distance;

                snapCoordinatorX = i;
            }
        }

        return snapPositionX;
    }

    private static float GetSnapPositionZ(float mousePositionZ, BoardProperty boardProperty, out int snapCoordinatorY)
    {
        snapCoordinatorY = 0;
        float snapPositionY = 0;
        float min = float.MaxValue;

        for (int i = 0; i < boardProperty.MaxRow; i++)
        {
            float tilePosition = boardProperty.FirstTilePosition.z + i * boardProperty.TileDistance.z;
            float distance = Mathf.Abs(mousePositionZ - tilePosition);

            if (distance < min)
            {
                snapPositionY = tilePosition;
                min = distance;

                snapCoordinatorY = i;
            }
        }

        return snapPositionY;
    }
}
