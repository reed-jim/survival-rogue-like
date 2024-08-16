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
    [SerializeField] private float playerLazyHpBarDuration;

    #region PRIVATE FIELD
    private Tween _lazyPlayerHpBarTween;
    private Tween _updateExpBarTween;
    #endregion

    private void Awake()
    {
        // StatManager.updateExpProgressBarEvent += UpdateExpProgressBar;
        CharacterStatManager.setHpEvent += UpdatePlayerHpBar;
        PlayerStat.updateExpProgressBarEvent += UpdateExpProgressBar;
    }

    private void OnDestroy()
    {
        // StatManager.updateExpProgressBarEvent -= UpdateExpProgressBar;
        CharacterStatManager.setHpEvent -= UpdatePlayerHpBar;
        PlayerStat.updateExpProgressBarEvent -= UpdateExpProgressBar;
    }

    private void UpdatePlayerHpBar(int instanceId, float prevHp, float currentHp, float maxHp)
    {
        playerHpProgressBar.value = currentHp / maxHp;

        float prevValue = playerLazyHpProgressBar.value;
        float currentValue = currentHp / maxHp;

        if (_lazyPlayerHpBarTween.isAlive)
        {
            _lazyPlayerHpBarTween.Stop();
        }

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
