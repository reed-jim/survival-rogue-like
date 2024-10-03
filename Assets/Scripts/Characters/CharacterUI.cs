using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    [Header("LAZY HP BAR")]
    [SerializeField] private LazyBar lazyHpBar;

    [Header("UI")]
    [SerializeField] private RectTransform container;
    [SerializeField] private Slider hpBar;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private RectTransform criticalDamageIcon;

    [Header("CUSTOMIZE")]
    [SerializeField] private float scaleDownDuration;

    [Header("MANAGEMENT")]
    private List<Tween> _tweens;

    private void Awake()
    {
        _tweens = new List<Tween>();

        CharacterStatManager.setHpEvent += SetHP;
        CharacterStatManager.showCriticalDamageEvent += HighlightCriticalDamage;

        criticalDamageIcon.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        lazyHpBar.gameObject.SetActive(false);
        damageText.gameObject.SetActive(false);
    }

    private void Update()
    {
        // lock rotation
        container.eulerAngles = Vector3.zero;
    }

    private void OnDestroy()
    {
        CharacterStatManager.setHpEvent -= SetHP;
        CharacterStatManager.showCriticalDamageEvent -= HighlightCriticalDamage;
    }

    public void Reset()
    {
        lazyHpBar.Reset();
    }

    public void SetHP(int instanceId, float prevValue, float value, float maxHp)
    {
        if (gameObject.GetInstanceID() == instanceId)
        {
            SetHP(prevValue, value, maxHp);
        }
    }

    public void SetHP(float prevValue, float value, float maxHp)
    {
        // _tweens.Add(Tween.Custom(prevValue / maxHp, value / maxHp, duration: 0.3f, onValueChange: newVal => hpBar.value = newVal));

        if (!lazyHpBar.gameObject.activeSelf)
        {
            ShowHpBar();
        }

        lazyHpBar.SetValue(prevValue, value, maxHp);

        float damage = prevValue - value;

        ShowDamage(damage);
    }

    public void ShowHpBar()
    {
        if (lazyHpBar != null)
        {
            lazyHpBar.gameObject.SetActive(true);
        }
    }

    public void HideHpBar()
    {
        // _tweens.Add(
        //     Tween.ScaleX(_hpBarRT, 0, duration: scaleDownDuration).OnComplete(() =>
        //     {
        //         _hpBarRT.localScale = Vector3.one;
        //         _hpBarRT.gameObject.SetActive(false);
        //     })
        // );
    }

    private void ShowDamage(float damage)
    {
        damageText.text = $"{damage}";
        damageText.gameObject.SetActive(true);

        Vector3 currentDamageTextPosition = damageText.rectTransform.localPosition;

        _tweens.Add(Tween.LocalPositionY(damageText.rectTransform, currentDamageTextPosition.y + 2f, duration: 0.3f));

        _tweens.Add(Tween.Delay(0.3f).OnComplete(() =>
        {
            _tweens.Add(
                Tween.Custom(1, 0, duration: 0.3f, onValueChange: newVal =>
                {
                    Color color = damageText.color;

                    color.a = newVal;

                    damageText.color = color;
                }
                ).OnComplete(() =>
            {
                damageText.gameObject.SetActive(false);

                damageText.rectTransform.localPosition = currentDamageTextPosition;
                damageText.color = ColorUtil.WithAlpha(damageText.color, 1);

                criticalDamageIcon.gameObject.SetActive(false);
            }));
        }));
    }

    private void HighlightCriticalDamage(int instanceId)
    {
        if (instanceId == gameObject.GetInstanceID())
        {
            criticalDamageIcon.gameObject.SetActive(true);
        }
    }
}
