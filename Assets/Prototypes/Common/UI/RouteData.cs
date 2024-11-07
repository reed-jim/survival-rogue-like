using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Route", menuName = "ScriptableObjects/RPG/RouteData")]
public class RouteData : ScriptableObject
{
    [SerializeField] private string routeName;
    [SerializeField] private Sprite iconSprite;
    [SerializeField] private bool isSelectedFromStart;

    public string RouteName => routeName;
    public Sprite IconSprite => iconSprite;
    public bool IsSelectedFromStart => isSelectedFromStart;
}
