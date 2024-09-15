using UnityEngine;
using UnityEngine.UI;

public class QuickPlayScreen : UIScreen
{
    [Header("UI ELEMENTS")]
    [SerializeField] private Button playButton;

    protected override void GenerateUI()
    {
        UIUtil.SetSize(playButton, 0.4f * _canvasSize.x, 0.15f * _canvasSize.x);
    }
}
