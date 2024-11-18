using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIManager : MonoBehaviour
{
    [SerializeField] private RectTransform canvas;
    [SerializeField] private RectTransform playerHpProgressBarRT;
    [SerializeField] private RectTransform playerLazyHpProgressBarRT;
    [SerializeField] private RectTransform expProgressBarRT;
    [SerializeField] private Slider playerHpProgressBar;
    [SerializeField] private Slider playerLazyHpProgressBar;
    [SerializeField] private Slider expProgressBar;
    [SerializeField] private TMP_Text playerHP;
    [SerializeField] private TMP_Text playerLevelText;

    [Header("CUSTOMIZE")]
    [SerializeField] private float playerHpBarDuration;
    [SerializeField] private float playerLazyHpBarDuration;

    #region PRIVATE FIELD
    private Vector2 _canvasSize;
    private Tween _playerHpBarTween;
    private Tween _lazyPlayerHpBarTween;
    private Tween _updateExpBarTween;
    #endregion

    private void Awake()
    {
        StatManager.updateExpProgressBarEvent += UpdateExpProgressBar;
        PlayerStatManager.setPlayerHpEvent += UpdatePlayerHpBar;
        StatManager.setPlayerHpEvent += UpdatePlayerHpBar;
        StatManager.updatePlayerLevelTextEvent += UpdateLevelText;

        _canvasSize = canvas.sizeDelta;

        GenerateUI();
    }

    private void OnDestroy()
    {
        StatManager.updateExpProgressBarEvent -= UpdateExpProgressBar;
        PlayerStatManager.setPlayerHpEvent -= UpdatePlayerHpBar;
        StatManager.setPlayerHpEvent -= UpdatePlayerHpBar;
        StatManager.updatePlayerLevelTextEvent -= UpdateLevelText;

        _playerHpBarTween.Stop();
        _lazyPlayerHpBarTween.Stop();
        _updateExpBarTween.Stop();
    }

    private void GenerateUI()
    {
        UIUtil.SetSize(expProgressBarRT, 0.8f * _canvasSize.x, 0.025f * _canvasSize.y);
        UIUtil.SetLocalPositionY(expProgressBarRT, 0.4f * _canvasSize.y);

        UIUtil.SetSize(playerHpProgressBarRT, expProgressBarRT.sizeDelta.x, 0.025f * _canvasSize.y);
        UIUtil.SetLocalPosition(playerHpProgressBarRT,
            0,
            expProgressBarRT.localPosition.y + 0.6f * (expProgressBarRT.sizeDelta.y + playerHpProgressBarRT.sizeDelta.y));

        // UIUtil.SetSize(playerHpProgressBarRT, 0.5f * _canvasSize.x, 0.025f * _canvasSize.y);
        // UIUtil.SetLocalPosition(playerHpProgressBarRT,
        //     -0.5f * (expProgressBarRT.sizeDelta.x - playerHpProgressBarRT.sizeDelta.x),
        //     expProgressBarRT.localPosition.y + 0.6f * (expProgressBarRT.sizeDelta.y + playerHpProgressBarRT.sizeDelta.y));

        UIUtil.SetSize(playerLazyHpProgressBarRT, playerHpProgressBarRT.sizeDelta);
        UIUtil.SetLocalPosition(playerLazyHpProgressBarRT, playerHpProgressBarRT.localPosition);
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

        playerHP.text = $"{(int)currentHp}/{(int)maxHp}";
    }

    private void UpdateExpProgressBar(float currentExp, float maxExp)
    {
        if (_updateExpBarTween.isAlive)
        {
            _updateExpBarTween.Stop();
        }

        _updateExpBarTween = Tween.Custom(expProgressBar.value, currentExp / maxExp, duration: 0.3f, onValueChange: newVal => expProgressBar.value = newVal);

        playerLevelText.text = $"{(int)currentExp}/{(int)maxExp}";
        // playerLevelText.text = $"Level ??? ({(int)currentExp}/{(int)maxExp})";
    }

    private void UpdateLevelText(int level, float currentExp, float requiredExp)
    {
        playerLevelText.text = $"Level {level} ({(int)currentExp}/{(int)requiredExp})";
    }
}
