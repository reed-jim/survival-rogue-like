using UnityEngine;

public class EntityMaterialPropertyBlock : MonoBehaviour
{
    [Header("CUSTOMIZE")]
    [SerializeField] private Texture2D mainTexture;
    [SerializeField] private float mainTextureWeight;
    [SerializeField] private Color color;

    private Renderer _renderer;
    private MaterialPropertyBlock _materialPropertyBlock;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _materialPropertyBlock = new MaterialPropertyBlock();

        SetColor();
    }

    private void OnValidate()
    {
        _renderer = GetComponent<Renderer>();
        _materialPropertyBlock = new MaterialPropertyBlock();

        SetColor();
    }

    private void SetColor()
    {
        _renderer.GetPropertyBlock(_materialPropertyBlock);

        if (mainTexture != null)
        {
            _materialPropertyBlock.SetTexture("_MainTexture", mainTexture);
        }

        _materialPropertyBlock.SetFloat("_MainTextureWeight", mainTextureWeight);
        _materialPropertyBlock.SetColor("_Color", color);

        _renderer.SetPropertyBlock(_materialPropertyBlock);
    }
}
