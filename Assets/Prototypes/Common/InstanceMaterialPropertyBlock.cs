using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceMaterialPropertyBlock : BaseInstanceMaterialPropertyBlock
{
    [Header("CUSTOMIZE")]
    [SerializeField] private Color color;
    [SerializeField] private string colorReference;

    private void OnValidate()
    {
        SetColor(color);
    }

    public void SetColor(Color color)
    {
        Init();

        _propertyBlock.SetColor(colorReference, color);

        _renderer.SetPropertyBlock(_propertyBlock);
    }
}
