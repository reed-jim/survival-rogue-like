using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("MANAGEMENT")]
    [SerializeField] private TrailRenderer bulletTrail;

    private void OnEnable()
    {
        Tween.Delay(0.5f).OnComplete(() => gameObject.SetActive(false));
    }

    private void OnDisable()
    {
        bulletTrail.Clear();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != Constants.PLAYER_TAG)
        {
            gameObject.SetActive(false);
        }
    }
}
