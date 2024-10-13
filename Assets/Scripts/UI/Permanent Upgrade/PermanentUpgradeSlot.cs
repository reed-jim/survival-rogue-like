using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PermanentUpgradeSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text statValueText;

    public void SetStatValue(float value)
    {
        statValueText.text = value.ToString();
    }
}
