using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class ExplosiveAreaIndicator : MonoBehaviour
{
    [SerializeField] private Transform explosiveArea;
    [SerializeField] private Transform lazyExplosiveArea;

    [Header("CUSTOMIZE")]
    [SerializeField] private float duration;
    [SerializeField] private float hideDuration;

    #region PRIVATE FIELD
    private List<Tween> _tweens;
    private SpriteRenderer _explosiveAreaRenderer;
    private SpriteRenderer _lazyExplosiveAreaRenderer;
    private bool _isBusy;
    #endregion

    private void Awake()
    {
        _tweens = new List<Tween>();
        _explosiveAreaRenderer = explosiveArea.GetComponent<SpriteRenderer>();
        _lazyExplosiveAreaRenderer = lazyExplosiveArea.GetComponent<SpriteRenderer>();

        explosiveArea.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        CommonUtil.StopAllTweens(_tweens);
    }

    public void ShowExplosiveArea(Vector3 position, float radius, float countdown)
    {
        if (_isBusy)
        {
            return;
        }
        else
        {
            _isBusy = true;
        }

        explosiveArea.gameObject.SetActive(true);

        float scaleMultiplier = 2 * radius / _explosiveAreaRenderer.bounds.size.x;

        explosiveArea.position = position + new Vector3(0, 0.5f, 0);
        explosiveArea.localScale = scaleMultiplier * Vector3.one;
        lazyExplosiveArea.localPosition = Vector3.zero;

        _explosiveAreaRenderer.color = CommonUtil.ChangeAlpha(Color.white, 0);

        _tweens.Add(Tween.Alpha(_explosiveAreaRenderer, 1, duration: countdown));

        lazyExplosiveArea.localScale = Vector3.zero;

        _tweens.Add(
            Tween.Scale(lazyExplosiveArea, 1, duration: duration)
            .OnComplete(() =>
            {
                _tweens.Add(Tween.Alpha(_explosiveAreaRenderer, 0, duration: hideDuration));
                _tweens.Add(Tween.Alpha(_lazyExplosiveAreaRenderer, 0, duration: hideDuration));
            })
        );
    }
}
