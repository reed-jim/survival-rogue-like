using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingWithICollectible : ObjectPoolingWithOneComponent<ICollectible>
{
    protected override void Awake()
    {
        base.Awake();

        CharacterCollectibleSpawner.getICollectibleEvent += GetComponentFromPool;
    }

    private void OnDestroy()
    {
        CharacterCollectibleSpawner.getICollectibleEvent -= GetComponentFromPool;
    }

    private void GetComponentRandomly()
    {

    }
}
