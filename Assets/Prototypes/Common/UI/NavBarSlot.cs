using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using Saferio.Util.SaferioTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NavBarSlot : MonoBehaviour
{
    [SerializeField] private RectTransform container;
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text routeName;
    [SerializeField] private Image containerImage;
    [SerializeField] private Image icon;

    [Header("DATA")]
    [SerializeField] private RouteData _routeData;

    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private SaferioTheme saferioTheme;

    #region PRIVATE FIELD
    private bool _isSelected;
    #endregion

    public static event Action<string> switchRoute;

    private void Awake()
    {
        switchRoute += OnRouteSwitched;

        button.onClick.AddListener(SwitchRoute);
    }

    private void OnDestroy()
    {
        switchRoute -= OnRouteSwitched;
    }

    public void SetUp(RouteData data)
    {
        _routeData = data;

        routeName.text = data.RouteName;

        icon.sprite = data.IconSprite;

        _isSelected = data.IsSelectedFromStart;

        if (_isSelected)
        {
            containerImage.color = saferioTheme.SelectedNavBarButtonColor;
        }
        else {
            containerImage.color = saferioTheme.UnselectedNavBarButtonColor;
        }

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

    private void OnRouteSwitched(string routeName)
    {
        if (!_isSelected)
        {
            if (routeName == _routeData.RouteName)
            {
                Tween.Color(containerImage, saferioTheme.SelectedNavBarButtonColor, duration: 0.5f);

                _isSelected = true;
            }
        }
        else
        {
            if (routeName != _routeData.RouteName)
            {
                Tween.Color(containerImage, saferioTheme.UnselectedNavBarButtonColor, duration: 0.5f)
                .OnComplete(() =>
                {
                    _isSelected = false;
                });
            }
        }
    }
}
