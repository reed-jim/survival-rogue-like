using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public static class CommonUtil
{
    public static void OnHitColorEffect(
        Renderer meshRenderer,
        MaterialPropertyBlock materialPropertyBlock,
        Color startColor,
        Color endColor,
        float duration,
        List<Tween> tweens
    )
    {
        Tween tween = Tween.Custom(
            startColor, endColor,
            duration: duration,
            onValueChange: newVal =>
            {
                materialPropertyBlock.SetColor("_Albedo", newVal);
                meshRenderer.SetPropertyBlock(materialPropertyBlock);
            },
            cycles: 2,
            cycleMode: CycleMode.Yoyo
            );

        tweens.Add(tween);
    }
}
