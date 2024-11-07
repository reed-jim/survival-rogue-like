using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaferioThemeManager : MonoBehaviour
{
    [SerializeField] private SaferioTheme[] someThemes;
    private int _currentThemeIndex;

    [SerializeField] private Button changeThemeButton;

    private void Awake()
    {
        changeThemeButton.onClick.AddListener(ChangeTheme);

        SetTheme();
    }

    public void SetTheme()
    {
        TMP_Text[] allTexts = FindObjectsOfType<TMP_Text>();

        foreach (var item in allTexts)
        {
            item.color = someThemes[_currentThemeIndex].TextColor;
        }

        ISaferioThemedElement[] themedElements =
            FindObjectsOfType<MonoBehaviour>()
            .OfType<ISaferioThemedElement>()
            .ToArray();

        foreach (var item in themedElements)
        {
            item.ApplyTheme(someThemes[_currentThemeIndex]);
        }
    }

    private void ChangeTheme()
    {
        _currentThemeIndex++;

        if (_currentThemeIndex >= someThemes.Length)
        {
            _currentThemeIndex = 0;
        }

        SetTheme();
    }
}
