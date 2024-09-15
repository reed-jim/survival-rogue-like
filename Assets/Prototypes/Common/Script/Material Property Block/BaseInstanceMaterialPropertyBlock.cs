using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInstanceMaterialPropertyBlock : MonoBehaviour
{
    protected Renderer _renderer;
    protected MaterialPropertyBlock _propertyBlock;

    protected void Init()
    {
        if (_renderer == null)
        {
            _renderer = GetComponent<Renderer>();
        }

        if (_propertyBlock == null)
        {
            _propertyBlock = new MaterialPropertyBlock();
        }
    }
}
