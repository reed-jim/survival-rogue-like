using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Saferio/SaferioTheme")]
public class SaferioTheme : ScriptableObject
{
    [SerializeField] private Color backgroundColor;
    [SerializeField] private Color textColor;
    [SerializeField] private Color titleColor;
    [SerializeField] private Color secondaryTextColor;
    [SerializeField] private Color unselectedNavBarButtonColor;
    [SerializeField] private Color selectedNavBarButtonColor;

    public Color BackgroundColor => backgroundColor;
    public Color TextColor => textColor;
    public Color TitleColor => titleColor;
    public Color SecondaryTextColor => secondaryTextColor;
    public Color SelectedNavBarButtonColor => selectedNavBarButtonColor;
    public Color UnselectedNavBarButtonColor => unselectedNavBarButtonColor;
}
