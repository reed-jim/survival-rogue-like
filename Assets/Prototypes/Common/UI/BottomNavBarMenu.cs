using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

public class BottomNavBarMenu : MonoBehaviour
{
    [Header("UI ELEMENTS")]
    [SerializeField] private RectTransform canvas;
    [SerializeField] private RectTransform slotPrefab;
    [SerializeField] private RectTransform slotContainer;
    [SerializeField] private RectTransform characterScreen;

    [Header("CUSTOMIZE")]
    [SerializeField] private int numSlot;

    [Header("DATA")]
    [SerializeField] private RouteDataContainer routeDataContainer;

    #region PRIVATE FIELD
    private RectTransform[] _slot;
    private Vector2 _canvasSize;
    #endregion

    private void Awake()
    {
        _canvasSize = canvas.sizeDelta;

        NavBarSlot.switchRoute += AnimationOnSwitchRoute;

        GenerateUI();
    }

    private void OnDestroy()
    {
        NavBarSlot.switchRoute -= AnimationOnSwitchRoute;
    }

    private void GenerateUI()
    {
        _slot = new RectTransform[numSlot];

        for (int i = 0; i < numSlot; i++)
        {
            _slot[i] = Instantiate(slotPrefab, slotContainer);

            UIUtil.SetSize(_slot[i], _canvasSize.x / numSlot, 0.08f * _canvasSize.y);
            UIUtil.SetLocalPositionX(_slot[i], -0.5f * (_canvasSize.x - _slot[i].sizeDelta.x) + i * _slot[i].sizeDelta.x);

            _slot[i].GetComponent<NavBarSlot>().SetUp(routeDataContainer.Items[i]);
        }

        UIUtil.SetSize(slotContainer, _canvasSize.x, _slot[0].sizeDelta.y);
        UIUtil.SetLocalPositionY(slotContainer, -0.5f * (_canvasSize.y - _slot[0].sizeDelta.y));

        new UISlideVertical().SlideIn(slotContainer, slotContainer.localPosition.y, slotContainer.localPosition.y - _slot[0].sizeDelta.y);
    }

    private void AnimationOnSwitchRoute(string route)
    {
        for (int i = 0; i < numSlot; i++)
        {
            if (route == routeDataContainer.Items[i].RouteName)
            {
                Tween.ScaleX(_slot[i], 1.2f, duration: 0.25f, cycleMode: CycleMode.Yoyo, cycles: 2);
            }
        }
    }
}
