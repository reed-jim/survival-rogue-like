using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider hpBar;

    [Header("MANAGEMENT")]
    private List<Tween> _tweens;

    private void Awake()
    {
        _tweens = new List<Tween>();
    }

    private void OnDestroy()
    {

    }

    public void SetHP(float prevValue, float value)
    {
        _tweens.Add(Tween.Custom(prevValue, value, duration: 0.3f, onValueChange: newVal => hpBar.value = newVal));
    }
}
