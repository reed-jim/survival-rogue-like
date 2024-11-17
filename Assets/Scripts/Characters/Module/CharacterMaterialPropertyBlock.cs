using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class CharacterMaterialPropertyBlock : MonoBehaviour
{
    [Header("CUSTOMIZE")]
    [SerializeField] private string colorReference;
    [SerializeField] private float hitEffectDuration;

    [SerializeField] private Renderer _renderer;
    private MaterialPropertyBlock _propertyBlock;
    private bool _isColorEffectActive;

    private void Awake()
    {
        MeleeWeaponCollider.characterHitEvent += ColorEffectOnHit;
        Bullet.characterHitEvent += ColorEffectOnHit;

        Init();
    }

    private void OnDestroy()
    {
        MeleeWeaponCollider.characterHitEvent += ColorEffectOnHit;
        Bullet.characterHitEvent -= ColorEffectOnHit;
    }

    private void Init()
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

    public void SetColor(Color color)
    {
        Init();

        _propertyBlock.SetColor(colorReference, color);

        _renderer.SetPropertyBlock(_propertyBlock);
    }

    private void ColorEffectOnHit(int instanceId)
    {
        if (_isColorEffectActive)
        {
            return;
        }

        if (instanceId == gameObject.GetInstanceID())
        {
            float hdrIntensity = 3;

            Tween.Custom(Color.white, Color.red, duration: hitEffectDuration, onValueChange: newVal => SetColor(hdrIntensity * newVal))
            .OnComplete(() =>
            {
                Tween.Custom(Color.red, Color.white, duration: hitEffectDuration, onValueChange: newVal => SetColor(newVal))
                .OnComplete(() =>
                {
                    _isColorEffectActive = false;
                });
            });

            _isColorEffectActive = true;
        }
    }
}
