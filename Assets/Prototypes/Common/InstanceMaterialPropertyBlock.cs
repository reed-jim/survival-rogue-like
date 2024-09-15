using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceMaterialPropertyBlock : MonoBehaviour
{
    [Header("CUSTOMIZE")]
    [SerializeField] private Color color;
    [SerializeField] private string colorReference;

    private Renderer _renderer;
    private MaterialPropertyBlock _propertyBlock;

    private void Awake()
    {

    }

    private void OnValidate()
    {
        _renderer = GetComponent<Renderer>();

        _propertyBlock = new MaterialPropertyBlock();

        SetColor(color);
    }

    public void Init()
    {
        _renderer = GetComponent<Renderer>();

        _propertyBlock = new MaterialPropertyBlock();
    }

    public void SetColor(Color color)
    {
        _propertyBlock.SetColor(colorReference, color);

        _renderer.SetPropertyBlock(_propertyBlock);
    }
}
