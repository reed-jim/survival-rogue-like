using System;
using System.Threading.Tasks;

#if UNITY_EDITOR
using Saferio.Editor.Style;
#endif
using Saferio.Util;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPoolingPageView : MonoBehaviour
{
    #region UI
    [SerializeField] private RectTransform itemPrefab;
    [SerializeField] private RectTransform container;
    [SerializeField] private RectTransform pageInputRT;
    [SerializeField] private RectTransform goToPageButtonRT;

    [SerializeField] private TMP_InputField pageInput;
    [SerializeField] private Button goToPageButton;
    [SerializeField] private Button nextPageButton;
    [SerializeField] private Button prevPageButton;
    #endregion

    #region PRIVATE FIELD
    private Vector2 _canvasSize;
    private RectTransform[] _items;
    private Button[] _itemButtons;
    private int _poolSize;

    [SerializeField] private int rowNumber;
    [SerializeField] private int columnNumber;
    [SerializeField] private int padding;
    [SerializeField] private int itemDistance;

    private int _currentPage;
    private ISaferioPageViewSlot[] _saferioPageViewSlots;
    #endregion

    #region ANIMATION
    private IPageViewAnimation _pageViewAnimation;
    #endregion

    #region EVENT
    public Action<int> OnPageSwitched;
    public Action<int> OnSlotSelected;
    #endregion

    #region PROPERTY
    /// <summary>
    /// Get current slots of the page view
    /// </summary>
    public RectTransform[] Items => _items;
    /// <summary>
    /// Get number of rows in the page view
    /// </summary>
    public int RowNumber => rowNumber;
    /// <summary>
    /// Get number of columns in the page view
    /// </summary>
    public int ColumnNumber => columnNumber;
    #endregion

    private async void Awake()
    {
        Spawn();

        await Task.Delay(2);
        GenerateUI();
        RegisterButton();

        SetUpPage();
    }

    private void RegisterEvent()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            int slotIndex = i;

            _itemButtons[i].onClick.AddListener(() => SelectSlot(slotIndex));
        }
    }

    private void GenerateUI()
    {
        Canvas canvas = container.GetComponentInParent<Canvas>();

        _canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;

        // UIUtil.SetSize(container, 0.9f * _canvasSize);

        Vector2 itemSize = Vector2.zero;
        Vector2 containerSize = container.sizeDelta;

        float totalDistanceBetweenItems = (_poolSize - 1) * itemDistance;

        int slotIndex;

        Vector2 slotSize = new Vector2(
            (containerSize.x - (2 * padding + (columnNumber - 1) * itemDistance)) / columnNumber,
            (containerSize.y - (2 * padding + (rowNumber - 1) * itemDistance)) / rowNumber
        );

        if (slotSize.x > slotSize.y)
        {
            slotSize.x = slotSize.y;
        }
        else
        {
            slotSize.y = slotSize.x;
        }

        for (int i = 0; i < rowNumber; i++)
        {
            for (int j = 0; j < columnNumber; j++)
            {
                slotIndex = j + columnNumber * i;

                UIUtil.SetSize(_items[slotIndex], slotSize);

                UIUtil.SetLocalPositionX(_items[slotIndex], (-(columnNumber - 1) / 2f + j) * (_items[slotIndex].sizeDelta.x + itemDistance));
                UIUtil.SetLocalPositionY(_items[slotIndex], ((rowNumber - 1) / 2f - i) * (_items[slotIndex].sizeDelta.y + itemDistance));
            }
        }

        UIUtil.SetSize(pageInputRT, 0.1f * containerSize.y, 0.05f * containerSize.y);
        UIUtil.SetLocalPositionY(pageInputRT, -0.5f * containerSize.y);

        UIUtil.SetSize(goToPageButton, 0.05f * containerSize.y, 0.05f * containerSize.y);
        UIUtil.SetLocalPosition(goToPageButtonRT, 0.25f * containerSize.x, pageInputRT.localPosition.y);

        UIUtil.SetSize(nextPageButton, 0.15f * containerSize.y, 0.15f * containerSize.y);
        UIUtil.SetLocalPosition(nextPageButton.GetComponent<RectTransform>(), 0.4f * containerSize.x, goToPageButtonRT.localPosition.y);

        UIUtil.SetSize(prevPageButton, 0.15f * containerSize.y, 0.15f * containerSize.y);
        UIUtil.SetLocalPosition(prevPageButton.GetComponent<RectTransform>(), -0.4f * containerSize.x, goToPageButtonRT.localPosition.y);
    }

    private void Spawn()
    {
        try
        {
            _poolSize = rowNumber * columnNumber;

            _items = new RectTransform[_poolSize];
            _itemButtons = new Button[_poolSize];
            _saferioPageViewSlots = new ISaferioPageViewSlot[_poolSize];

            for (int i = 0; i < _poolSize; i++)
            {
                _items[i] = Instantiate(itemPrefab, container);
                _items[i].name = $"Item {i}";

                _itemButtons[i] = _items[i].GetComponent<Button>();
                _saferioPageViewSlots[i] = _items[i].GetComponent<ISaferioPageViewSlot>();
            }
        }
        catch (Exception e)
        {
            DebugUtil.DistinctLog(e);
        }
    }

    private void RegisterButton()
    {
        goToPageButton.onClick.AddListener(GoToPage);
        nextPageButton.onClick.AddListener(NextPage);
        prevPageButton.onClick.AddListener(PrevPage);

        for (int i = 0; i < _poolSize; i++)
        {
            int slotIndex = i;

            _itemButtons[i].onClick.AddListener(() => SelectSlot(slotIndex));
        }
    }

    #region PAGE
    private void SetUpPage()
    {
        try
        {
            int currentStartSlotIndex = _currentPage * _poolSize;
            int currentSlotIndex = currentStartSlotIndex;

            for (int i = 0; i < _saferioPageViewSlots.Length; i++)
            {
                currentSlotIndex = currentStartSlotIndex + i;

                _saferioPageViewSlots[i].Setup(currentSlotIndex);
            }
        }
        catch (Exception e)
        {
            
        }
    }

    private void GoToPage()
    {
        GoToPage(int.Parse(pageInput.text));
    }

    private void GoToPage(int page)
    {
        _currentPage = page;

        _currentPage = Mathf.Max(0, _currentPage);

        SetUpPage();

        if (_pageViewAnimation != null)
        {
            _pageViewAnimation.PlayPageTransition();
        }

        OnPageSwitched?.Invoke(page);
    }

    private void NextPage()
    {
        GoToPage(_currentPage + 1);
    }

    private void PrevPage()
    {
        GoToPage(_currentPage - 1);
    }

    private void SelectSlot(int slotIndex)
    {
        OnSlotSelected?.Invoke(slotIndex);
    }
    #endregion

    #region ANIMATION
    /// <summary>
    /// Apply custom animation for page transition
    /// </summary>
    /// <param name="pageViewAnimation">The custom animation to apply</param>
    public void ApplyAnimation(IPageViewAnimation pageViewAnimation)
    {
        _pageViewAnimation = pageViewAnimation;
    }
    #endregion
}

#region INSPECTOR
#if UNITY_EDITOR
[CustomEditor(typeof(ObjectPoolingPageView))]
public class ObjectPoolingPageViewEditor : Editor
{
    private SerializedProperty container;
    private SerializedProperty itemPrefab;
    private SerializedProperty pageInputRT;
    private SerializedProperty goToPageButtonRT;

    private SerializedProperty pageInput;
    private SerializedProperty goToPageButton;
    private SerializedProperty nextPageButton;
    private SerializedProperty prevPageButton;

    private SerializedProperty rowNumber;
    private SerializedProperty columnNumber;
    private SerializedProperty padding;
    private SerializedProperty itemDistance;

    private SerializedProperty pageViewAnimation;

    private void OnEnable()
    {
        container = serializedObject.FindProperty("container");
        itemPrefab = serializedObject.FindProperty("itemPrefab");
        pageInputRT = serializedObject.FindProperty("pageInputRT");
        goToPageButtonRT = serializedObject.FindProperty("goToPageButtonRT");

        pageInput = serializedObject.FindProperty("pageInput");
        goToPageButton = serializedObject.FindProperty("goToPageButton");
        nextPageButton = serializedObject.FindProperty("nextPageButton");
        prevPageButton = serializedObject.FindProperty("prevPageButton");

        rowNumber = serializedObject.FindProperty("rowNumber");
        columnNumber = serializedObject.FindProperty("columnNumber");
        padding = serializedObject.FindProperty("padding");
        itemDistance = serializedObject.FindProperty("itemDistance");

        pageViewAnimation = serializedObject.FindProperty("pageViewAnimation");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        container.objectReferenceValue = EditorUtil.StyledObjectField("Container", container.objectReferenceValue, SaferioEditorStyle.LabelStyle());
        itemPrefab.objectReferenceValue = EditorUtil.StyledObjectField("Item Prefab", itemPrefab.objectReferenceValue, SaferioEditorStyle.LabelStyle());
        pageInputRT.objectReferenceValue = EditorUtil.StyledObjectField("Page Input RT", pageInputRT.objectReferenceValue, SaferioEditorStyle.LabelStyle());
        goToPageButtonRT.objectReferenceValue = EditorUtil.StyledObjectField("Go to Page RT", goToPageButtonRT.objectReferenceValue, SaferioEditorStyle.LabelStyle());

        pageInput.objectReferenceValue = EditorUtil.StyledObjectField("Page Input", pageInput.objectReferenceValue, SaferioEditorStyle.LabelStyle());
        goToPageButton.objectReferenceValue = EditorUtil.StyledObjectField("Go to Page Button", goToPageButton.objectReferenceValue, SaferioEditorStyle.LabelStyle());

        nextPageButton.objectReferenceValue = EditorUtil.StyledObjectField("Next Page Button", nextPageButton.objectReferenceValue, SaferioEditorStyle.LabelStyle());
        prevPageButton.objectReferenceValue = EditorUtil.StyledObjectField("Prev Page Button", prevPageButton.objectReferenceValue, SaferioEditorStyle.LabelStyle());

        SaferioEditorStyle.HeaderWithSpace("Customize");

        rowNumber.intValue = (int)EditorUtil.StyledNumberField("Row Number", rowNumber.intValue, SaferioEditorStyle.LabelStyle(), SaferioEditorStyle.TextFieldStyle());
        columnNumber.intValue = (int)EditorUtil.StyledNumberField("Column Number", columnNumber.intValue, SaferioEditorStyle.LabelStyle(), SaferioEditorStyle.TextFieldStyle());
        padding.intValue = (int)EditorUtil.StyledNumberField("Padding", padding.intValue, SaferioEditorStyle.LabelStyle(), SaferioEditorStyle.TextFieldStyle());
        itemDistance.intValue = (int)EditorUtil.StyledNumberField("Item Distance", itemDistance.intValue, SaferioEditorStyle.LabelStyle(), SaferioEditorStyle.TextFieldStyle());

        SaferioEditorStyle.HeaderWithSpace("Animation (Transition)");

        // pageViewAnimation.objectReferenceValue =
        //     EditorUtil.StyledObjectField("Page View Animation", pageViewAnimation.objectReferenceValue, SaferioEditorStyle.LabelStyle());

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
#endregion
