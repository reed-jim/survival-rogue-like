using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using TMPro;
using UnityEngine;

public static class TMPUtil
{
    public static void TweenFontSize(TMP_Text text, float fontSize, float duration)
    {
        float startValue = text.fontSize;
        float endValue = fontSize;

        Tween.Custom(startValue, endValue, duration: duration, onValueChange: newVal => text.fontSize = newVal);
    }

    public static void TweenTextAlpha(TMP_Text text, float alpha, float duration)
    {
        Tween.Alpha(text, alpha, duration: duration);
    }

    public static void TweenTextCharacterSpacing(TMP_Text text, float endSpacing, float duration)
    {
        float startValue = text.lineSpacing;
        float endValue = endSpacing;

        Tween.Custom(startValue, endValue, duration: duration, onValueChange: newVal => text.lineSpacing = newVal);
    }

    public static void TweenTextWordSpacing(TMP_Text text, float endSpacing, float duration)
    {
        float startValue = text.wordSpacing;
        float endValue = endSpacing;

        Tween.Custom(startValue, endValue, duration: duration, onValueChange: newVal => text.wordSpacing = newVal);
    }
}
