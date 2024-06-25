using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource hitSound;

    private void Awake()
    {
        Enemy.hitEvent += PlayHitSound;
    }

    private void OnDestroy()
    {
        Enemy.hitEvent -= PlayHitSound;
    }

    private void PlayHitSound()
    {
        hitSound.Play();
    }
}
