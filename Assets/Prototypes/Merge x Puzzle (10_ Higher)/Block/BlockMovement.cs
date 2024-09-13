using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

namespace Puzzle.Merge
{
    public class BlockMovement : MonoBehaviour
    {
        [Header("SCRIPTABLE OBJECT")]
        [SerializeField] private BoardProperty boardProperty;

        [Header("CUSTOMIZE")]
        [SerializeField] private float speedMultiplier;

        #region PRIVATE FIELD
        private Rigidbody _rigidBody;
        private int _cachedInstanceId;
        #endregion

        private void Awake()
        {
            RegisterEvents();

            _rigidBody = GetComponent<Rigidbody>();

            _cachedInstanceId = gameObject.GetInstanceID();
        }

        private void OnDestroy()
        {
            UnregisterEvents();
        }

        private void RegisterEvents()
        {
            InputManager.moveBlockEvent += Move;
        }

        private void UnregisterEvents()
        {
            InputManager.moveBlockEvent -= Move;
        }

        private void Move(int instanceID, Vector3 direction)
        {
            if (instanceID == _cachedInstanceId)
            {
                Move(direction);

                // _rigidBody.velocity = speedMultiplier * direction;

                // StartCoroutine(CheckBlockStop());
            }
        }

        private Vector2 GetCurrentTile(Vector3 direction)
        {
            int originalLayer = gameObject.layer;

            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

            Vector2 currentTileCoordinator = new Vector2();

            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 10))
            {
                ITile tile = hit.collider.GetComponent<ITile>();

                currentTileCoordinator = new Vector2(tile.X, tile.Y);

            }

            gameObject.layer = originalLayer;

            return currentTileCoordinator;
        }

        private Vector2 GetNextTile(Vector3 direction)
        {
            Vector2 nextTileCoordinator = GetCurrentTile(direction);

            if (direction == Vector3.forward)
            {
                nextTileCoordinator.y = boardProperty.MaxRow - 1;
            }
            else if (direction == Vector3.right)
            {
                nextTileCoordinator.x = boardProperty.MaxColumn - 1;
            }
            else if (direction == Vector3.back)
            {
                nextTileCoordinator.y = 0;
            }
            else
            {
                nextTileCoordinator.x = 0;
            }

            return nextTileCoordinator;
        }

        private void Move(Vector3 direction)
        {
            Vector3 endPosition = TileMapUtil.GetPositionFromCoordinator(GetNextTile(direction), boardProperty.FirstTilePosition, boardProperty.TileDistance);

            endPosition.y = transform.position.y;

            Tween.Position(transform, endPosition, duration: 1f / speedMultiplier);
        }
    }
}
