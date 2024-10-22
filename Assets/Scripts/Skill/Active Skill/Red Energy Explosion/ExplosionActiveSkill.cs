using System;
using ExitGames.Client.Photon.StructWrapping;
using PrimeTween;
using ReedJim.RPG.Stat;
using Saferio.Util.SaferioTween;
using UnityEngine;
using static CustomDelegate;

[CreateAssetMenu(menuName = "ScriptableObjects/RPG/ExplosionActiveSkill")]
public class ExplosionActiveSkill : BaseActiveSkill, IActiveSkill
{
    [SerializeField] private PlayerRuntime playerRuntime;

    #region PRIVATE FIELD
    private int _casterInstanceId;
    #endregion

    #region ACTION
    public static event GetVector3Action<int> getEnemyPositionEvent;
    #endregion

    #region IActiveSkill Implement
    public override void Cast()
    {
        _casterInstanceId = playerRuntime.PlayerInstanceId;

        EnergyExplosion explosion = ObjectPoolingEverything.GetFromPool(SaferioPrefabIdentifier.Explosion.ToString()).GetComponent<EnergyExplosion>();

        explosion.SetStat(Stat);

        explosion.gameObject.SetActive(true);

        Vector3 targetPosition = getEnemyPositionEvent.Invoke(_casterInstanceId);

        explosion.transform.position = targetPosition;

        IsCountdown = true;

        SaferioTween.Delay(4, onCompletedAction: () => IsCountdown = false);

        // Tween.Delay(Stat.GetStatValue(StatComponentNameConstant.AttackSpeed))
        //     .OnComplete(() => IsCountdown = false);
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
        return "Red Energy";
    }

    public override string GetName()
    {
        return "Red Energy";
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
