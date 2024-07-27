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

    public void SetHP(float prevValue, float value, float maxHp)
    {
        _tweens.Add(Tween.Custom(prevValue / maxHp, value / maxHp, duration: 0.3f, onValueChange: newVal => hpBar.value = newVal));
    }

    public void ShowHpBar()
    {
        if (_hpBarRT != null)
        {
            _hpBarRT.gameObject.SetActive(true);
        }
    }

    public void HideHpBar()
    {
        _tweens.Add(
            Tween.ScaleX(_hpBarRT, 0, duration: scaleDownDuration).OnComplete(() =>
            {
                _hpBarRT.localScale = Vector3.one;
                _hpBarRT.gameObject.SetActive(false);
            })
        );
    }
}
