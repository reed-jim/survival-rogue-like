using System;
using System.Collections;
using System.Collections.Generic;
using ReedJim.RPG.Stat;
using UnityEngine;

public class BaseActiveSkill : BaseSkill, IActiveSkill
{
    [Header("STAT")]
    [SerializeField] private PredifinedCharacterStat skillStat;

    #region PRIVATE FIELD
    private bool _isUnlocked;
    private bool _isCountdown = false;
    #endregion

    public static event Action<int, IActiveSkill> addActiveSkillEvent;

    public bool IsSkillUnlocked
    {
        get => _isUnlocked;
        set => _isUnlocked = value;
    }

    public bool IsCountdown
    {
        get => _isCountdown;
        set => _isCountdown = value;
    }

    public CharacterStat Stat => skillStat.GetBaseCharacterStat();

    protected void InvokeAddActiveSkillEvent(int instanceID)
    {
        addActiveSkillEvent?.Invoke(instanceID, this);
    }
    
    public virtual void Cast()
    {
        throw new NotImplementedException();
    }

    public virtual bool IsInCountdown()
    {
        throw new NotImplementedException();
    }

    public virtual bool IsUnlocked()
    {
        throw new NotImplementedException();
    }


    public override void AddSkill()
    {
        throw new System.NotImplementedException();
    }

    public override string GetDescription()
    {
        throw new System.NotImplementedException();
    }

    public override string GetName()
    {
        throw new System.NotImplementedException();
    }

    public override int GetTier()
    {
        throw new System.NotImplementedException();
    }
}
