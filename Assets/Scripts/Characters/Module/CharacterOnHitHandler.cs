using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterOnHitHandler : MonoBehaviour
{
    private bool _isInAnimation;

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
        if (instanceId == gameObject.GetInstanceID() && !_isInAnimation)
        {
            StartCoroutine
            (
                SpringAnimation.SpringScaleAnimation
                (
                    transform, 0.02f * Vector3.one, 1f, 4, 0.1f, onCompletedAction: () => _isInAnimation = false
                )
            );

            _isInAnimation = true;
        }
    }
}
