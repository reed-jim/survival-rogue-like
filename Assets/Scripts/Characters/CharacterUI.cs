using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using Saferio.Util.SaferioTween;
using TMPro;
using Unity.VisualScripting;
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

    [Header("MANAGEMENT")]
    private List<Tween> _tweens;
    private Tween colorTween;
    private bool _isInAnimation;
    private float _cumulativeDamage;
    private Vector3 _initialDamageTextScale;

    private void Awake()
    {
        _tweens = new List<Tween>();

        CharacterStatManager.setHpEvent += SetHP;
        CharacterStatManager.showCriticalDamageEvent += HighlightCriticalDamage;
        CharacterStatManager.characterDieEvent += HideDamageText;

        criticalDamageIcon.gameObject.SetActive(false);

        _initialDamageTextScale = damageText.rectTransform.localScale;
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
        CharacterStatManager.characterDieEvent -= HideDamageText;

        _cumulativeDamage = 0;
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

    IEnumerator TweenText(TMP_Text text, int start, int end)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(Time.deltaTime);

        int current = start;

        while (current < end)
        {
            current += 1;

            text.text = $"{current}";

            yield return waitForSeconds;
        }
    }

    private void ShowDamage(float damage)
    {
        if (_isInAnimation)
        {
            return;
        }

        damageText.text = $"{damage}";
        damageText.gameObject.SetActive(true);

        _cumulativeDamage += damage;

        damageText.text = $"{_cumulativeDamage}";

        if (colorTween.isAlive)
        {
            colorTween.Stop();

            damageText.color = ColorUtil.WithAlpha(damageText.color, 1);

            StartCoroutine(TweenText(damageText, (int)(_cumulativeDamage - damage), (int)_cumulativeDamage));
        }

        colorTween = Tween.Delay(0.4f).OnComplete(() =>
        {
            Tween.Alpha(damageText, 0, duration: 0.5f).OnComplete(() =>
            {
                damageText.gameObject.SetActive(false);
                damageText.color = ColorUtil.WithAlpha(damageText.color, 1);

                _isInAnimation = false;

                _cumulativeDamage = 0;
            });

            _isInAnimation = true;
        });
    }

    private void HideDamageText(int instanceId)
    {
        if (instanceId == gameObject.GetInstanceID())
        {
            Tween.Alpha(damageText, 0, duration: 0.5f).OnComplete(() =>
            {
                damageText.gameObject.SetActive(false);
                damageText.color = ColorUtil.WithAlpha(damageText.color, 1);
            });

            lazyHpBar.Hide();
        }
    }

    private void HighlightCriticalDamage(int instanceId)
    {
        if (instanceId == gameObject.GetInstanceID())
        {
            Tween.Scale(damageText.rectTransform, 1.5f * _initialDamageTextScale, duration: 0.2f);
        }
    }
}
