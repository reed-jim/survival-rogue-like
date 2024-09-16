using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class ShowOnLevelStart : MonoBehaviour
{
    [SerializeField] private RectTransform canvas;
    [SerializeField] private RectTransform target;

    [Header("CUSTOMIZE")]
    [SerializeField] private float transitionDuration;

    private Tween _tween;
    private Vector2 _canvasSize;
    private Vector2 _cachedPosition;

    private void Awake()
    {
        LevelSpawner.startLevelEvent += Show;

        _canvasSize = canvas.sizeDelta;

        StartCoroutine(DelayCachePosition());
    }

    private IEnumerator DelayCachePosition()
    {
        yield return new WaitForSeconds(2f);

        _cachedPosition = target.localPosition;
    }

    private void OnDestroy()
    {
        if (_tween.isAlive)
        {
            _tween.Stop();
        }

        LevelSpawner.startLevelEvent -= Show;
    }

    private void Show()
    {
        if (target.gameObject.activeSelf)
        {
            return;
        }

        target.gameObject.SetActive(true);

        _tween = Tween.LocalPositionY(target, _cachedPosition.y, duration: transitionDuration);
    }
}
