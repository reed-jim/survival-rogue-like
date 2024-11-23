using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider playerHpProgressBar;
    [SerializeField] private Slider playerLazyHpProgressBar;
    [SerializeField] private TMP_Text playerHP;

    [Header("CUSTOMIZE")]
    [SerializeField] private float playerHpBarDuration;
    [SerializeField] private float playerLazyHpBarDuration;

    #region PRIVATE FIELD
    private Tween _playerHpBarTween;
    private Tween _lazyPlayerHpBarTween;
    #endregion

    private void Awake()
    {
        PlayerStatManager.setPlayerHpEvent += UpdatePlayerHpBar;
        StatManager.setPlayerHpEvent += UpdatePlayerHpBar;
    }

    private void OnDestroy()
    {
        PlayerStatManager.setPlayerHpEvent -= UpdatePlayerHpBar;
        StatManager.setPlayerHpEvent -= UpdatePlayerHpBar;

        _playerHpBarTween.Stop();
        _lazyPlayerHpBarTween.Stop();
    }

    private void UpdatePlayerHpBar(float currentHp, float maxHp)
    {
        float prevValue = playerLazyHpProgressBar.value;
        float currentValue = currentHp / maxHp;

        if (_playerHpBarTween.isAlive)
        {
            _playerHpBarTween.Stop();
        }

        if (_lazyPlayerHpBarTween.isAlive)
        {
            _lazyPlayerHpBarTween.Stop();
        }

        _playerHpBarTween = Tween.Custom(prevValue, currentValue, duration: playerHpBarDuration, onValueChange: newVal => playerHpProgressBar.value = newVal);
        _lazyPlayerHpBarTween = Tween.Custom(prevValue, currentValue, duration: playerLazyHpBarDuration, onValueChange: newVal => playerLazyHpProgressBar.value = newVal);

        // playerHP.text = $"{(int)currentHp}/{(int)maxHp}";
        playerHP.text = $"{(int)currentHp}";
    }
}
