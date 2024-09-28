using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public static class CameraUtil
{
    public static void CameraShake(Camera camera, Vector3 amplitude, float duration)
    {
        SpringAnimation.SpringPositionAnimation(camera.transform, amplitude, 0.1f, 6, duration);
    }

    public static void Fade(SpriteRenderer fadeImage, float duration, float delayTimeFadeOut)
    {
        fadeImage.gameObject.SetActive(true);

        Color color = fadeImage.color;

        color.a = 0;

        fadeImage.color = color;

        FadeIn(fadeImage, duration, onCompleteAction: () =>
        {
            Tween.Delay(delayTimeFadeOut).OnComplete(() => FadeOut(fadeImage, duration));
        });
    }

    public static void FadeIn(SpriteRenderer fadeImage, float duration, Action onCompleteAction)
    {
        Tween.Alpha(fadeImage, 1, duration: duration)
            .OnComplete(() => onCompleteAction.Invoke());
    }

    public static void FadeOut(SpriteRenderer fadeImage, float duration)
    {
        Tween.Alpha(fadeImage, 0, duration: duration);
    }

    public static void TweenFieldOfView(Camera camera, float duration)
    {
        float startValue = camera.fieldOfView;
        float endValue = 0.7f * camera.fieldOfView;

        Tween.Custom(startValue, endValue, duration: duration, onValueChange: newVal => camera.fieldOfView = newVal);
    }

    public static void TweenOrthographicSize(Camera camera, float duration)
    {
        float startValue = camera.fieldOfView;
        float endValue = 0.7f * camera.fieldOfView;

        Tween.Custom(startValue, endValue, duration: duration, onValueChange: newVal => camera.orthographicSize = newVal);
    }
}
