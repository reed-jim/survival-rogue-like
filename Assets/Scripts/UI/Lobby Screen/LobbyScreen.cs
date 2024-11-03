using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyScreen : UIScreen
{
    [SerializeField] private LobbyNetworkManager lobbyNetworkManager;

    [SerializeField] private RectTransform createRoomButtonRT;
    [SerializeField] private RectTransform joinRoomButtonRT;
    [SerializeField] private RectTransform refreshLobbyButtonRT;

    [SerializeField] private Button createRoomButton;
    [SerializeField] private Button joinRoomButton;
    [SerializeField] private Button refreshLobbyButton;

    #region PRIVATE FIELD
    private int _currentRoomIndex;
    #endregion

    #region ACTION
    public static event Action<int, string> updateLobbyRoomEvent;
    public static event Action refreshLobbyListEvent;
    #endregion

    public override void RegisterEvent()
    {
        base.RegisterEvent();

        createRoomButton.onClick.AddListener(CreateRoom);
        joinRoomButton.onClick.AddListener(JoinRoom);
        refreshLobbyButton.onClick.AddListener(RefreshLobbyList);
    }

    public override void UnregisterEvent()
    {

    }

    protected override void GenerateUI()
    {
        UIUtil.SetSizeKeepRatioX(createRoomButtonRT, 0.05f * _canvasSize.y);
        UIUtil.SetSize(joinRoomButtonRT, createRoomButtonRT.sizeDelta);
        UIUtil.SetSizeKeepRatioX(refreshLobbyButtonRT, createRoomButtonRT.sizeDelta.y);

        UIUtil.SetLocalPosition(joinRoomButtonRT, 0.3f * _canvasSize.x, 0.3f * _canvasSize.y);
        UIUtil.SetLocalPositionOfRectToAnotherRectHorizontally(createRoomButtonRT, joinRoomButtonRT, -0.55f, -0.55f);
        UIUtil.SetLocalPositionOfRectToAnotherRectHorizontally(refreshLobbyButtonRT, createRoomButtonRT, -0.55f, -0.55f);
    }

    private void CreateRoom()
    {
        lobbyNetworkManager.CreatePublicLobbyAsync();
    }

    private void JoinRoom()
    {
        // lobbyNetworkManager.JoinRoom();
    }

    // private void OnHostStarted(string joinCode)
    // {
    //     updateLobbyRoomEvent?.Invoke(_currentRoomIndex, joinCode);

    //     _currentRoomIndex++;
    // }

    private void RefreshLobbyList()
    {
        refreshLobbyListEvent?.Invoke();
    }
}
