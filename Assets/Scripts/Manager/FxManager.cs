using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] hitFXs;
    [SerializeField] private ParticleSystem[] bulletHitFXs;

    [Header("CUSTOMIZE")]
    private int _currentHitFXIndex;
    private int _currentBulletHitFXIndex;

    private void Awake()
    {
        Enemy.playHitFxEvent += PlayHitFX;
        Enemy.playBulletHitFxEvent += PlayHitFX;
    }

    private void OnDestroy()
    {
        Enemy.playHitFxEvent -= PlayHitFX;
        Enemy.playBulletHitFxEvent -= PlayHitFX;
    }

    private void PlayHitFX(Vector3 hitPosition)
    {
        hitFXs[_currentHitFXIndex].transform.position = hitPosition;
        hitFXs[_currentHitFXIndex].gameObject.SetActive(true);
        hitFXs[_currentHitFXIndex].Play();

        _currentHitFXIndex++;

        if (_currentHitFXIndex >= hitFXs.Length)
        {
            _currentHitFXIndex = 0;
        }
    }

    private void PlayBulletHitFX(Vector3 hitPosition)
    {
        bulletHitFXs[_currentBulletHitFXIndex].transform.position = hitPosition;
        bulletHitFXs[_currentBulletHitFXIndex].gameObject.SetActive(true);
        bulletHitFXs[_currentBulletHitFXIndex].Play();

        _currentBulletHitFXIndex++;

        if (_currentBulletHitFXIndex >= bulletHitFXs.Length)
        {
            _currentBulletHitFXIndex = 0;
        }
    }
}
