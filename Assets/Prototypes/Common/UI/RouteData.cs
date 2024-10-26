using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Route", menuName = "ScriptableObjects/RPG/RouteData")]
public class RouteData : ScriptableObject
{
    [SerializeField] private string routeName;
    [SerializeField] private Sprite iconSprite;

    public string RouteName => routeName;
    public Sprite IconSprite => iconSprite;
}
