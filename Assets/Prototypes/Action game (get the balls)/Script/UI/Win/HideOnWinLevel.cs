using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using Prototypes.Merge;
using UnityEngine;

public class HideOnWinLevel : MonoBehaviour
{
    [SerializeField] private RectTransform canvas;
    [SerializeField] private RectTransform target;

    [Header("CUSTOMIZE")]
    [SerializeField] private float transitionDuration;

    private Tween _tween;
    private Vector2 _canvasSize;

    private void Awake()
    {
        GameScoreManager.winLevelEvent += Hide;

        _canvasSize = canvas.sizeDelta;
    }

    private void OnDestroy()
    {
        if (_tween.isAlive)
        {
            _tween.Stop();
        }

        GameScoreManager.winLevelEvent -= Hide;
    }

    private void Hide()
    {
        if (target.localPosition.y > 0)
        {
            _tween = Tween.LocalPositionY(target, _canvasSize.y, duration: transitionDuration)
            .OnComplete(() => target.gameObject.SetActive(false));
        }
        else
        {
            _tween = Tween.LocalPositionY(target, -_canvasSize.y, duration: transitionDuration)
            .OnComplete(() => target.gameObject.SetActive(false));
        }
    }
}
