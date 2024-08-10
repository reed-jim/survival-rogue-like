using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

public class LazyBar : MonoBehaviour
{
    [SerializeField] private Slider bar;
    [SerializeField] private Slider lazyBar;

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

        _tweens.Add(Tween.Custom(prevValue / maxValue, value / maxValue, duration: 0.5f, onValueChange: newVal => lazyBar.value = newVal));
    }
}
