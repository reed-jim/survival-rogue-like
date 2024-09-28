using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PrimeTween;
using Saferio.Util;
using UnityEngine.UI;

public class InGameLogger : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private ObjectPoolingScrollView objectPoolingScrollView;
    [SerializeField] private RectTransform iconBorder;
    [SerializeField] private RectTransform icon;
    [SerializeField] private Button changeVisibilityButton;

    #region PRIVATE FIELD
    private List<string> _cachedLogs;
    #endregion

    public static event Action<string> updateLogEvent;

    private void Awake()
    {
        _cachedLogs = new List<string>();

        Application.logMessageReceived += DisplayLog;

        changeVisibilityButton.onClick.AddListener(ChangeVisibility);

        Tween.Delay(0.3f).OnComplete(() =>
        {
            GenerateUI();

            // SampleLog();
        });
    }

    private void OnDestroy()
    {
        Application.logMessageReceived -= DisplayLog;
    }

    private void SampleLog()
    {
        for (int i = 0; i < 9; i++)
        {
            DebugUtil.DistinctLog($"Sample Log {i}");
        }
    }

    private void GenerateUI()
    {
        RectTransform changeVisibilityButtonRT = changeVisibilityButton.GetComponent<RectTransform>();

        UIUtil.SetLocalPosition(changeVisibilityButtonRT, 0.51f * (objectPoolingScrollView.ViewArea.sizeDelta + changeVisibilityButtonRT.sizeDelta));
        UIUtil.SetLocalPositionX(changeVisibilityButtonRT, 0);

        UIUtil.SetSizeKeepRatioX(iconBorder, changeVisibilityButtonRT.sizeDelta.y);
        UIUtil.SetLocalPosition(iconBorder, -0.5f * (objectPoolingScrollView.ViewArea.sizeDelta.x - iconBorder.sizeDelta.x), changeVisibilityButtonRT.localPosition.y);

        UIUtil.SetSizeKeepRatioX(icon, 0.8f * iconBorder.sizeDelta.y);
    }

    private void DisplayLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Warning)
        {
            return;
        }

        objectPoolingScrollView.AddItem(logString);

        updateLogEvent?.Invoke(logString);
    }

    private void ChangeVisibility()
    {
        GameObject viewArea = objectPoolingScrollView.ViewArea.gameObject;

        viewArea.SetActive(!viewArea.activeSelf);
    }
}
