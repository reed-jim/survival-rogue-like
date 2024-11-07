using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class QuickPlayScreen : UIScreen
{
    [Header("UI ELEMENTS")]
    [SerializeField] private RectTransform gameName;
    [SerializeField] private RectTransform playButtonRT;
    [SerializeField] private Button playButton;
    [SerializeField] private TMP_Text gameNameText;
    [SerializeField] private TMP_Text playText;

    protected override void GenerateUI()
    {
        UIUtil.SetLocalPositionY(gameName, 0.3f * _canvasSize.y);

        UIUtil.SetSize(playButtonRT, 0.4f * _canvasSize.x, 0.15f * _canvasSize.x);
        UIUtil.SetLocalPositionY(playButtonRT, -0.2f * _canvasSize.y);

        gameNameText.fontSize = 0.08f * _canvasSize.y;
        playText.fontSize = 0.03f * _canvasSize.y;

        RegisterButton();
    }

    private void RegisterButton()
    {
        playButton.onClick.AddListener(GoToGameplay);
    }

    private void GoToGameplay()
    {
        Addressables.LoadSceneAsync("Gameplay");
    }
}
