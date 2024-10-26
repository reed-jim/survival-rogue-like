using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/RPG/RotateBladeSkill")]
public class RotateBladeSkill : BaseActiveSkill, IActiveSkill
{
    [SerializeField] private PlayerRuntime playerRuntime;

    #region PRIVATE FIELD
    private int _casterInstanceId;
    #endregion

    #region IActiveSkill Implement
    public override void Cast()
    {
        _casterInstanceId = playerRuntime.PlayerInstanceId;

        RotateBlade rotateBlade = ObjectPoolingEverything.GetFromPool(GetName()).GetComponent<RotateBlade>();

        rotateBlade.SetStat(Stat);

        rotateBlade.gameObject.SetActive(true);
        rotateBlade.transform.SetParent(playerRuntime.player);
        rotateBlade.transform.localPosition = new Vector3(0, 1, 0);

        IsCountdown = true;
    }

    public override bool IsUnlocked()
    {
        return IsSkillUnlocked;
    }

    public override bool IsInCountdown()
    {
        return IsCountdown;
    }
    #endregion

    public override string GetDescription()
    {
        return "Rotate Blade";
    }

    public override string GetName()
    {
        return name;
    }

    public override int GetTier()
    {
        return 1;
    }

    public override void AddSkill()
    {
        InvokeAddActiveSkillEvent(_casterInstanceId);

        IsCountdown = false;

        IsSkillUnlocked = true;
    }
}
