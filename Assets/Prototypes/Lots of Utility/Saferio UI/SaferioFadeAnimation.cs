using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct SaferioFadeAnimation : ISaferioUIAnimation
{
    [SerializeField] private Image target;
    private Color _initialColor;

    public void Play()
    {
        _initialColor = target.color;

        target.color = ColorUtil.WithAlpha(target.color, 0);

        FadeAsync();
    }

    public async void FadeAsync()
    {
        Color difference = _initialColor - target.color;

        int maxStep = 20;
        int step = 0;

        Color deltaColor = difference / maxStep;

        while (step < maxStep)
        {
            target.color += deltaColor;

            step++;

            await Task.Delay(20);
        }
    }
}
