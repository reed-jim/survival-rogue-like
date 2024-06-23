using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public static class TransformUtil
{
    public static void RotateRight(Transform target, List<Tween> tweens, float duration = 0.2f, Action onCompletedAction = null)
    {
        Vector3 currentEulerAngles = target.eulerAngles;

        tweens.Add(
            Tween.Rotation(target, currentEulerAngles + new Vector3(0, 90, 0), duration: duration).OnComplete(() =>
            {
                onCompletedAction?.Invoke();
            })
        );
    }

    public static void RotateLeft(Transform target, List<Tween> tweens, float duration = 0.2f, Action onCompletedAction = null)
    {
        Vector3 currentEulerAngles = target.eulerAngles;

        tweens.Add(
            Tween.Rotation(target, currentEulerAngles - new Vector3(0, 90, 0), duration: duration).OnComplete(() =>
            {
                onCompletedAction?.Invoke();
            })
        );
    }
}
