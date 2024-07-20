using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] hitFXs;

    [Header("CUSTOMIZE")]
    private int _currentHitFXIndex;

    private void Awake()
    {
        Enemy.playHitFxEvent += PlayHitFX;
    }

    private void OnDestroy()
    {
        Enemy.playHitFxEvent -= PlayHitFX;
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
}
