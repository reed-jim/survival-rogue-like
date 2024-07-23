using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIManager : MonoBehaviour
{
    [SerializeField] private Slider expProgressBar;

    [SerializeField] private PlayerStat playerStat;

    private void Awake()
    {
        StatManager.updateExpProgressBarEvent += UpdateExpProgressBar;
        PlayerStat.updateExpProgressBarEvent += UpdateExpProgressBar;
    }

    private void OnDestroy()
    {
        StatManager.updateExpProgressBarEvent -= UpdateExpProgressBar;
        PlayerStat.updateExpProgressBarEvent -= UpdateExpProgressBar;
    }

    private void UpdateExpProgressBar(float currentExp)
    {
        expProgressBar.value = currentExp / playerStat.GetRequiredExpForNextLevel();
    }
}
