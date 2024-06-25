using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshColliderUpdater : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<SkinnedMeshRenderer>();
        collider = GetComponent<MeshCollider>();
    }

    private float time = 0;
    // Update is called once per frame
    void Update()
    {
        UpdateCollider();

        // time += Time.deltaTime;

        // if (time >= 0.02f)
        // {
        //     time = 0;
        //     UpdateCollider();
        // }
    }

    SkinnedMeshRenderer meshRenderer;
    MeshCollider collider;

    public void UpdateCollider()
    {
        Mesh colliderMesh = new Mesh();
        meshRenderer.BakeMesh(colliderMesh);
        collider.sharedMesh = null;
        collider.sharedMesh = colliderMesh;
    }
}
