using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class CollectibleExperienceShard : MonoBehaviour, ICollectible
{
    [SerializeField] private Collider collectibleCollider;

    #region PRIVATE FIELD
    private List<Tween> _tweens;
    private Vector3 _initialScale;
    #endregion

    #region ACTION
    public static Action<int> earnPlayerExperienceEvent;
    #endregion  
    private void Awake()
    {
        _tweens = new List<Tween>();

        gameObject.SetActive(false);

        _initialScale = transform.localScale;
    }

    private void OnEnable()
    {
        collectibleCollider.enabled = true;

        transform.localScale = _initialScale;
    }

    public void Collect()
    {
        earnPlayerExperienceEvent?.Invoke(1);

        collectibleCollider.enabled = false;

        _tweens.Add(Tween.PositionY(transform, transform.position.y + 5, duration: 0.4f));
        _tweens.Add(Tween.Scale(transform, 0, duration: 0.4f)
        .OnComplete(() => gameObject.SetActive(false)));
    }

    public void Spawn(Vector3 position)
    {
        gameObject.transform.position = position;

        gameObject.SetActive(true);
    }
}
