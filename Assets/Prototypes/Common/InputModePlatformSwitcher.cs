using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputModePlatformSwitcher : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button switchButton;
    [SerializeField] private TMP_Text inputModeText;

    private void Awake()
    {
        switchButton.onClick.AddListener(Switch);
    }

    private void Switch()
    {

    }
}
