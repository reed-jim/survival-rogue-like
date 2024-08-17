using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIManager : MonoBehaviour
{
    [SerializeField] private Slider playerHpProgressBar;
    [SerializeField] private Slider playerLazyHpProgressBar;
    [SerializeField] private Slider expProgressBar;

    [Header("CUSTOMIZE")]
    [SerializeField] private float playerHpBarDuration;
    [SerializeField] private float playerLazyHpBarDuration;

    #region PRIVATE FIELD
    private Tween _playerHpBarTween;
    private Tween _lazyPlayerHpBarTween;
    private Tween _updateExpBarTween;
    #endregion

    private void Awake()
    {
        StatManager.updateExpProgressBarEvent += UpdateExpProgressBar;
        PlayerStatManager.setPlayerHpEvent += UpdatePlayerHpBar;
        StatManager.setPlayerHpEvent += UpdatePlayerHpBar;
    }

    private void OnDestroy()
    {
        StatManager.updateExpProgressBarEvent -= UpdateExpProgressBar;
        PlayerStatManager.setPlayerHpEvent -= UpdatePlayerHpBar;
        StatManager.setPlayerHpEvent -= UpdatePlayerHpBar;

        _playerHpBarTween.Stop();
        _lazyPlayerHpBarTween.Stop();
        _updateExpBarTween.Stop();
    }

    private void UpdatePlayerHpBar(float currentHp, float maxHp)
    {
        // playerHpProgressBar.value = currentHp / maxHp;

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
    }

    private void UpdateExpProgressBar(float currentExp, float maxExp)
    {
        if (_updateExpBarTween.isAlive)
        {
            _updateExpBarTween.Stop();
        }

        _updateExpBarTween = Tween.Custom(expProgressBar.value, currentExp / maxExp, duration: 0.3f, onValueChange: newVal => expProgressBar.value = newVal);
    }
}
