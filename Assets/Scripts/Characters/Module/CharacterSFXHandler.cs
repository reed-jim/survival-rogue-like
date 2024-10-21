using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSFXHandler : MonoBehaviour
{
    [SerializeField] private AudioSource attackSFX;

    private void Awake()
    {
        PlayerAttack.playAttackSFXEvent += PlayAttackSFX;
    }

    private void OnDestroy()
    {
        PlayerAttack.playAttackSFXEvent -= PlayAttackSFX;
    }

    private void PlayAttackSFX(int instanceId)
    {
        if (instanceId == gameObject.GetInstanceID())
        {
            attackSFX.Play();
        }
    }
}
