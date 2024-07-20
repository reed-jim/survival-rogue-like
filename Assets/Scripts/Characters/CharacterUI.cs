using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider hpBar;

    [Header("CUSTOMIZE")]
    [SerializeField] private float scaleDownDuration;

    [Header("MANAGEMENT")]
    private List<Tween> _tweens;
    private RectTransform _hpBarRT;

    private void Awake()
    {
        _tweens = new List<Tween>();

        _hpBarRT = hpBar.GetComponent<RectTransform>();
    }

    private void Update()
    {
        // lock rotation
        transform.eulerAngles = Vector3.zero;
    }

    private void OnDestroy()
    {

    }

    public void SetHP(float prevValue, float value)
    {
        _tweens.Add(Tween.Custom(prevValue, value, duration: 0.3f, onValueChange: newVal => hpBar.value = newVal));
    }

    public void HideHpBar()
    {
        _tweens.Add(Tween.ScaleX(_hpBarRT, 0, duration: scaleDownDuration));
    }
}
