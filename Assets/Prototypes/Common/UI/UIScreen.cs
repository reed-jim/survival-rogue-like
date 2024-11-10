using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScreen : MonoBehaviour, IUIScreen, ISubcriber
{
    [SerializeField] protected RectTransform canvas;
    [SerializeField] protected RectTransform container;

    [Header("CUSTOMIZE")]
    [SerializeField] private bool isDisableScreenFromStart;

    [Header("TEMP")]
    [SerializeField] private string routeName;

    #region PRIVATE FIELD
    protected Vector2 _canvasSize;
    #endregion

    #region SLIDE TRANSITION
    protected IUISlide _UISlide;
    #endregion

    protected virtual void Awake()
    {
        RegisterEvent();

        InitVariable();

        GenerateUI();

        if (isDisableScreenFromStart)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        UpdateState();
    }

    private void OnDestroy()
    {
        UnregisterEvent();
    }

    protected virtual void InitVariable()
    {
        _canvasSize = canvas.sizeDelta;

        _UISlide = new UISlideHorizontal();
    }

    protected virtual void GenerateUI()
    {

    }

    #region IUIScreen Interface
    public virtual void Hide()
    {
        new UISlideHorizontal().SlideOut(container, container.localPosition.x, -_canvasSize.x);
    }

    public virtual void Show()
    {
        new UISlideHorizontal().SlideIn(container, container.localPosition.x, _canvasSize.x);
    }

    public virtual void UpdateState()
    {

    }
    #endregion

    private void OnRouteSwitched(string route)
    {
        if (route == routeName)
        {
            if (!gameObject.activeSelf)
            {
                Show();
            }
        }
        else
        {
            if (gameObject.activeSelf)
            {
                Hide();
            }
        }
    }

    #region ISubcriber Interface
    public virtual void RegisterEvent()
    {
        NavBarSlot.switchRoute += OnRouteSwitched;
        LobbyNetworkManager.switchRoute += OnRouteSwitched;
    }

    public virtual void UnregisterEvent()
    {
        NavBarSlot.switchRoute -= OnRouteSwitched;
        LobbyNetworkManager.switchRoute -= OnRouteSwitched;
    }
    #endregion
}
