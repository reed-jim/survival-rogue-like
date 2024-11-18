using System;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class QuickPlayScreen : UIScreen
{
    [Header("UI ELEMENTS")]
    [SerializeField] private RectTransform gameName;
    [SerializeField] private RectTransform playButtonRT;
    [SerializeField] private RectTransform serverConnectStateTextRT;
    [SerializeField] private Button playButton;
    [SerializeField] private TMP_Text gameNameText;
    [SerializeField] private TMP_Text playText;
    [SerializeField] private TMP_Text serverConnectStateText;

    #region ACTION
    public static event Action startSingleplayEvent;
    #endregion

    protected override void GenerateUI()
    {
        UIUtil.SetLocalPositionY(gameName, 0.3f * _canvasSize.y);

        UIUtil.SetSize(playButtonRT, 0.4f * _canvasSize.x, 0.15f * _canvasSize.x);
        UIUtil.SetLocalPositionY(playButtonRT, -0.2f * _canvasSize.y);

        UIUtil.SetSize(serverConnectStateTextRT, _canvasSize.x, 0.1f * _canvasSize.x);
        UIUtil.SetLocalPositionOfRectToAnotherRectVertically(serverConnectStateTextRT, playButtonRT, -0.5f, -0.7f);

        gameNameText.fontSize = 0.08f * _canvasSize.y;
        playText.fontSize = 0.03f * _canvasSize.y;
        UIUtil.SetFontSizeOnly(serverConnectStateText, 0.03f * _canvasSize.y);

        RegisterButton();
    }

    private void RegisterButton()
    {
        playButton.onClick.AddListener(StartSingleplay);
    }

    private void GoToGameplay()
    {
        Addressables.LoadSceneAsync("Gameplay");
    }

    private void StartSingleplay()
    {
        startSingleplayEvent?.Invoke();

        serverConnectStateText.text = $"Connecting...";
    }
}
