using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using static CustomDelegate;

public class BlockSpawner : MonoBehaviour
{
    public static event GetTransformAction getTransformEvent;
    public static event GetVector3Action getSpawnPositionEvent;
    public static event Action<int> snapBlockEvent;

    private void Awake()
    {
        Tween.Delay(0.5f).OnComplete(() => SpawnBlock());
    }

    private void SpawnBlock()
    {
        Transform block = getTransformEvent?.Invoke();

        block.gameObject.SetActive(true);

        block.position = getSpawnPositionEvent.Invoke() + new Vector3(0, 10, 0);

        snapBlockEvent?.Invoke(block.gameObject.GetInstanceID());
    }
}
