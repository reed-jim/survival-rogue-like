using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCollision : MonoBehaviour
{
    [Header("COMPONENT")]
    [SerializeField] private Rigidbody blockRigidbody;
    [SerializeField] private MeshRenderer blockRenderer;

    [Header("CUSTOMIZE")]
    [SerializeField] private float maxRaycastDistance;

    #region PRIVATE FIELD
    private int _cachedInstanceId;
    #endregion

    private void Awake()
    {
        BlockSpawner.snapBlockEvent += Snap;

        _cachedInstanceId = gameObject.GetInstanceID();
    }

    private void OnDestroy()
    {
        BlockSpawner.snapBlockEvent -= Snap;
    }

    private void OnTriggerEnter(Collider other)
    {
        // blockRigidbody.velocity = Vector3.zero;

        // Snap();
    }

    private void Snap()
    {
        int originalLayer = gameObject.layer;

        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, maxRaycastDistance))
        {
            if (hit.collider.GetComponent<ITile>() != null)
            {
                transform.position = new Vector3(hit.collider.transform.position.x, hit.point.y + 0.6f * blockRenderer.bounds.size.y, hit.collider.transform.position.z);
            }
        }

        gameObject.layer = originalLayer;
    }

    private void Snap(int instanceId)
    {
        if (instanceId == _cachedInstanceId)
        {
            Snap();
        }
    }
}
