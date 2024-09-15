using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradientMaterialCustomizer : BaseInstanceMaterialPropertyBlock
{
    [Header("CUSTOMIZE")]
    [SerializeField] private Color color1;
    [SerializeField] private Color color2;
    [SerializeField] private float blendHeight;

    private void Awake()
    {
        SetGradient(color1, color2, blendHeight);
    }

    private void OnValidate()
    {
        SetGradient(color1, color2, blendHeight);
    }

    public void SetGradient(Color color1, Color color2, float blendHeight)
    {
        Init();

        _propertyBlock.SetColor("_Color1", color1);
        _propertyBlock.SetColor("_Color2", color2);
        _propertyBlock.SetFloat("_BlendHeight", blendHeight);

        _renderer.SetPropertyBlock(_propertyBlock);
    }
}
