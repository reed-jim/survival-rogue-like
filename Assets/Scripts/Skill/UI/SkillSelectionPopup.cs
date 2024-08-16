using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelectionPopup : MonoBehaviour, IUITransition
{
    [SerializeField] private RectTransform canvas;
    [SerializeField] private RectTransform container;
    [SerializeField] private Button[] selectButtons;
    [SerializeField] private TMP_Text[] skillDescriptions;

    private RectTransform[] _selectButtonRTs;

    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private SkillContainer skillContainer;

    private Vector2 _canvasSize;
    private ISkill[] skillsToChoose;

    private void Awake()
    {
        foreach (var item in skillContainer.AllSkills)
        {
            Debug.Log(item.GetDescription());
        }

        _canvasSize = canvas.sizeDelta;

        _selectButtonRTs = new RectTransform[selectButtons.Length];
        skillsToChoose = new ISkill[selectButtons.Length];

        for (int i = 0; i < selectButtons.Length; i++)
        {
            _selectButtonRTs[i] = selectButtons[i].GetComponent<RectTransform>();
        }

        GenerateUI();

        Show();
    }

    private void GenerateUI()
    {
        container.sizeDelta = new Vector2(0.8f * _canvasSize.x, 0.8f * _canvasSize.y);

        for (int i = 0; i < selectButtons.Length; i++)
        {
            int index = i;

            _selectButtonRTs[i].sizeDelta = new Vector2(0.3f * container.sizeDelta.x, 0.6f * container.sizeDelta.y);
            _selectButtonRTs[i].localPosition = new Vector2((i - 1) * 1.05f * _selectButtonRTs[i].sizeDelta.x, 0);

            selectButtons[i].onClick.AddListener(() => SelectSkill(index));
        }
    }

    private void SelectSkill(int slotIndex)
    {
        Debug.Log(skillsToChoose[slotIndex].GetDescription());
    }

    public void Hide()
    {
        throw new System.NotImplementedException();
    }

    public void Show()
    {
        for (int i = 0; i < selectButtons.Length; i++)
        {
            skillsToChoose[i] = RandomSkillMachine.GetRandomSkill(skillContainer);
            skillDescriptions[i].text = skillsToChoose[i].GetDescription();
        }

        Vector3 startPositionY = container.localPosition + new Vector3(0, _canvasSize.y, 0);
        float endPositionY = container.localPosition.y;

        container.localPosition = startPositionY;

        Tween.LocalPositionY(container, endPositionY, duration: 0.5f);
    }
}
