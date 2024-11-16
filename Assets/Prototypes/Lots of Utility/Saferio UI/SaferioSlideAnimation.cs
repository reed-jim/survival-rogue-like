using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class SaferioSlideAnimation : ISaferioUIAnimation
{
    [SerializeField] private RectTransform target;
    private Vector3 _initialPosition;

    public void Play()
    {
        _initialPosition = target.localPosition;

        target.localPosition += new Vector3(1000, 0, 0);

        SlideAsync();
    }

    public async void SlideAsync()
    {
        Vector3 distance = _initialPosition - target.localPosition;

        int maxStep = 20;
        int step = 0;

        Vector3 deltaPosition = distance / maxStep;

        while (step < maxStep)
        {
            target.localPosition += deltaPosition;

            step++;

            await Task.Delay(20);
        }
    }
}
