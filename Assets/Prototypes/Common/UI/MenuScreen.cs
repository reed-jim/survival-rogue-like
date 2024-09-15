using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScreen : UIScreen
{
    [Header("UI ELEMENTS")]
    [SerializeField] private Button playButton;

    protected override void GenerateUI()
    {
        UIUtil.SetSize(playButton, 0.4f * _canvasSize.x, 0.15f * _canvasSize.x);
    }
}
