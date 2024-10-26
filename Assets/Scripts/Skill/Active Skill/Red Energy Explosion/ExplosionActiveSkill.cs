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

        EnergyExplosion explosion = ObjectPoolingEverything.GetFromPool(GetName()).GetComponent<EnergyExplosion>();

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
        string description = $"{name}\n";

        string rarityTierColor = SurvivoriumTheme.RARITY_COLORs[_rarityTier];

        foreach (var statComponent in Stat.StatComponents)
        {
            if (statComponent.Value.BaseValue > 0)
            {
                description += $"<color=#fff>{statComponent.Key} - <color={rarityTierColor}>{statComponent.Value.BaseValue}</color>\n";
            }
        }

        return description;
    }

    public override string GetName()
    {
        return name;
    }

    public override int GetTier()
    {
        _rarityTier = UnityEngine.Random.Range(0, 5);

        return _rarityTier;
    }

    public override void AddSkill()
    {
        InvokeAddActiveSkillEvent(_casterInstanceId);

        IsCountdown = false;

        IsSkillUnlocked = true;
    }
}
