using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Route Data Container", menuName = "ScriptableObjects/RPG/RouteDataContainer")]
public class RouteDataContainer : ScriptableObject
{
    [SerializeField] private RouteData[] items;

    public RouteData[] Items => items;
}
