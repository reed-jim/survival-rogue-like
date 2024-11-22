using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    [SerializeField] private GameObject mapPrefab;

    private void Awake()
    {
        Instantiate(mapPrefab);
    }
}
