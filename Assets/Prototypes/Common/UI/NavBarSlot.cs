using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NavBarSlot : MonoBehaviour
{
    [SerializeField] private RectTransform container;
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text routeName;
    [SerializeField] private Image icon;

    [Header("DATA")]
    [SerializeField] private RouteData _routeData;

    public static event Action<string> switchRoute;

    private void Awake()
    {
        button.onClick.AddListener(SwitchRoute);
    }

    public void SetUp(RouteData data)
    {
        _routeData = data;

        routeName.text = data.RouteName;

        icon.sprite = data.IconSprite;

        GenerateUI();
    }

    private void GenerateUI()
    {
        UIUtil.SetSizeKeepRatioY(icon.rectTransform, 0.4f * container.sizeDelta.x);
    }

    private void SwitchRoute()
    {
        switchRoute?.Invoke(_routeData.RouteName);
    }
}
