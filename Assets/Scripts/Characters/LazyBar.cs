using System.Collections.Generic;
using PrimeTween;
using Saferio.Util.SaferioTween;
using UnityEngine;
using UnityEngine.UI;

public class LazyBar : MonoBehaviour
{
    [SerializeField] private Slider bar;
    [SerializeField] private Slider lazyBar;

    [SerializeField] private RectTransform barRT;
    [SerializeField] private RectTransform lazyBarRT;

    [Header("MANAGEMENT")]
    private List<Tween> _tweens;

    private void Awake()
    {
        _tweens = new List<Tween>();

        Reset();
    }

    public void Reset()
    {
        bar.value = 1;
        lazyBar.value = 1;
    }

    public void SetValue(float prevValue, float value, float maxValue)
    {
        bar.value = value / maxValue;

        _tweens.Add(
            Tween.Custom(prevValue / maxValue, value / maxValue, duration: 0.5f, onValueChange: newVal => lazyBar.value = newVal)
            .OnComplete(() =>
            {
                if (value <= 0)
                {
                    gameObject.SetActive(false);
                }
            })
        );
    }

    public void Hide()
    {
        Vector3 initialScale = barRT.localScale;

        SaferioTween.ScaleX(barRT, 0, 0.3f, onCompletedAction: () =>
        {
            barRT.localScale = initialScale;

            barRT.gameObject.SetActive(false);
        });

        SaferioTween.ScaleX(lazyBarRT, 0, 0.3f, onCompletedAction: () =>
        {
            lazyBarRT.localScale = initialScale;

            lazyBarRT.gameObject.SetActive(false);
        });
    }
}
