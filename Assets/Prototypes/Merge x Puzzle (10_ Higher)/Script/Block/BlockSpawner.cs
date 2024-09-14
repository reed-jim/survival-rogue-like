using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using static CustomDelegate;

namespace Puzzle.Merge
{
    public class BlockSpawner : MonoBehaviour
    {
        [Header("SCRIPTABLE OBJECT")]
        [SerializeField] private BoardProperty boardProperty;

        public static event GetTransformAction getTransformEvent;
        public static event GetVector3Action getSpawnPositionEvent;
        public static event Action<int> snapBlockEvent;
        public static event Action<int, Vector2Int, Vector3> moveBlockEvent;

        private void Awake()
        {
            InputManager.spawnBlockEvent += SpawnBlockThenMove;
        }

        private void OnDestroy()
        {
            InputManager.spawnBlockEvent -= SpawnBlockThenMove;
        }

        private void SpawnBlock()
        {
            Transform block = getTransformEvent?.Invoke();

            block.gameObject.SetActive(true);

            block.position = getSpawnPositionEvent.Invoke() + new Vector3(0, 10, 0);

            snapBlockEvent?.Invoke(block.gameObject.GetInstanceID());
        }

        private void SpawnBlockThenMove(Vector3 mousePosition, Vector3 direction)
        {
            Transform block = getTransformEvent?.Invoke();

            block.gameObject.SetActive(true);

            TileLocation location = TileMapUtil.GetSnapPosition(mousePosition, boardProperty);

            block.position = location.Position + new Vector3(0, 1, 0);

            moveBlockEvent?.Invoke(block.gameObject.GetInstanceID(), location.Coordinator, direction);
        }
    }
}
