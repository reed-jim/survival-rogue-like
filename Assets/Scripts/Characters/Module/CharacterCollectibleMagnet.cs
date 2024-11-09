using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterCollectibleMagnet : MonoBehaviour
{
    [SerializeField] private ChildColliderObserver magnetColliderObserver;

    [Header("CUSTOMIZE")]
    [SerializeField] private string[] filteredTags;

    private void Awake()
    {
        magnetColliderObserver.RegisterOnTriggerEvent(HandleOnMagnetTriggerEnter);
    }

    private void HandleOnMagnetTriggerEnter(Collider other)
    {
        try
        {
            if (!filteredTags.Contains(other.tag))
            {
                return;
            }

            ICollectible collectible = other.gameObject.GetComponent<ICollectible>();

            if (collectible != null)
            {
                collectible.Collect();
            }
        }
        catch
        {

        }
    }

    // private void OnCollisionEnter(Collision collision)
    // {
    // ICollectible collectible = collision.gameObject.GetComponent<ICollectible>();

    // if (collectible != null)
    // {
    //     collectible.Collect();
    // }
    // }
}
