using System.Collections;
using System.Collections.Generic;
using ReedJim.RPG.Stat;
using UnityEngine;

public class BaseActiveSkill : MonoBehaviour
{
    [Header("STAT")]
    [SerializeField] private PredifinedCharacterStat skillStat;

    #region PRIVATE FIELD
    private bool _isCountdown;
    #endregion

    public bool IsCountdown
    {
        get => _isCountdown;
        set => _isCountdown = value;
    }

    public CharacterStat Stat => skillStat.GetBaseCharacterStat();
}
