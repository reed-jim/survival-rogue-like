using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using Saferio.Util.SaferioTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private RectTransform canvas;
    [SerializeField] private RectTransform loadingTextRT;
    [SerializeField] private Image fadeBackground;
    [SerializeField] private TMP_Text loadingText;

    [Header("CUSTOMIZE")]
    [SerializeField] private float startDelay;
    [SerializeField] private float fadeOutDuration;

    private Vector2 _canvasSize;

    private void Awake()
    {
        _canvasSize = canvas.sizeDelta;

        GenerateUI();

        Transition();
    }

    private void GenerateUI()
    {
        UIUtil.SetLocalPositionY(loadingTextRT, -0.35f * _canvasSize.y);

        UIUtil.SetSizeX(loadingTextRT, _canvasSize.x);
        UIUtil.SetFontSizeOnly(loadingText, 0.03f * _canvasSize.y);
    }

    private void Transition()
    {
        SaferioTween.Delay(startDelay, onCompletedAction: (() =>
        {
            SaferioTween.AlphaAsync(fadeBackground, 0, duration: fadeOutDuration);
            SaferioTween.LocalPositionAsync(loadingTextRT, new Vector3(0, -_canvasSize.y), duration: fadeOutDuration);
        }));
    }
}
