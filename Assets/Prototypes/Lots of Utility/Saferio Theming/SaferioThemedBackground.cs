using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaferioThemedBackground : MonoBehaviour, ISaferioThemedElement
{
    [SerializeField] private Image background;

    public void ApplyTheme(SaferioTheme theme)
    {
        background.color = theme.BackgroundColor;
    }
}
