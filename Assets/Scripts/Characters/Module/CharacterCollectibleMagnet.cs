using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollectibleMagnet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        ICollectible collectible = collision.gameObject.GetComponent<ICollectible>();

        if (collectible != null)
        {
            collectible.Collect();
        }
    }
}
