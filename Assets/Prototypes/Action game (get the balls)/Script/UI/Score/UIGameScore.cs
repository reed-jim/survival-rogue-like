using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using Prototypes.Merge;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameScore : MonoBehaviour, ITweenDisposable
{
    [Header("UI")]
    [SerializeField] private RectTransform canvas;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private Slider progressBar;
    [SerializeField] private RectTransform progressBarRT;

    [Header("CUSTOMIZE")]
    [SerializeField] private float progressBarTransitionDuration;

    #region PRIVATE FIELD
    private List<Tween> _tweens;
    private Vector2 _canvasSize;
    #endregion

    private void Awake()
    {
        GameScoreManager.setScoreEvent += SetScore;

        _tweens = new List<Tween>();

        _canvasSize = canvas.sizeDelta;

        Init();

        GenerateUI();
    }

    private void OnDestroy()
    {
        GameScoreManager.setScoreEvent -= SetScore;
    }

    private void Init()
    {
        SetScore(0, 36);
    }

    private void GenerateUI()
    {
        UIUtil.SetSize(progressBarRT, 0.7f * _canvasSize.x, 0.7f / 12 * _canvasSize.x);
        UIUtil.SetLocalPositionY(progressBarRT, 0.4f * _canvasSize.y);
        
        UIUtil.SetTextSize(scoreText, 0.015f * _canvasSize.y);
        UIUtil.SetSize(scoreText.rectTransform, progressBarRT.sizeDelta);
    }

    private void SetScore(int currentScore, int requiredScore)
    {
        float percent = (float)currentScore / requiredScore * 100;

        scoreText.text = $"{currentScore}/{requiredScore} ({percent}%)";

        Tween.Custom(progressBar.value, percent / 100f, duration: progressBarTransitionDuration, onValueChange: newVal =>
        {
            progressBar.value = newVal;
        });
    }

    #region ITweenDisposable
    public void StopAllTweens()
    {
        CommonUtil.StopAllTweens(_tweens);
    }
    #endregion
}
