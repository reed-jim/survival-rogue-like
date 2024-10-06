using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterOnHitHandler : MonoBehaviour
{
    private Coroutine _onHitEffectCoroutine;

    private void Awake()
    {
        MeleeWeapon.characterHitEvent += OnHit;
    }

    private void OnDestroy()
    {
        MeleeWeapon.characterHitEvent -= OnHit;
    }

    private void OnHit(int instanceId)
    {
        if (instanceId == gameObject.GetInstanceID() && _onHitEffectCoroutine == null)
        {
            _onHitEffectCoroutine = StartCoroutine(SpringAnimation.SpringScaleAnimation(transform, 0.05f * Vector3.one, 1f, 4, 0.1f));
        }
    }
}
