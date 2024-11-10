using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using ReedJim.RPG.Stat;
using UnityEngine;

public class PlayerStatManager : CharacterStatManager
{
    private PlayerStat _playerStat;

    public override CharacterStat Stat { get => _playerStat; }

    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private EquipmentSkillObserver equipmentSkillObserver;

    #region ACTION
    public static event Action<int, PlayerStat> addPlayerStatToListEvent;
    public static event Action<float, float> setPlayerHpEvent;
    #endregion

    // protected override void OnEnable()
    // {
    //     base.OnEnable();

    //     Tween.Delay(0.5f).OnComplete(() =>
    //     {
    //         addPlayerStatToListEvent?.Invoke(_playerStat);

    //         InvokeUpdateHPBarEvent(_playerStat.MaxHP);
    //     });
    // }

    protected override void InitializeStat()
    {
        CharacterStat characterStat = CharacterStat.Load(Constants.PLAYER_TAG, baseStat.GetBaseCharacterStat());

        _playerStat = new PlayerStat(characterStat);

        AddSkillFromEquipments();

        Tween.Delay(0.5f).OnComplete(() =>
        {
            addPlayerStatToListEvent?.Invoke(gameObject.GetInstanceID(), _playerStat);

            IStatComponent health = _playerStat.GetStat(StatComponentNameConstant.Health);

            InvokeUpdateHPBarEvent(health.BaseValue);
        });
    }

    private void AddSkillFromEquipments()
    {
        foreach (var skill in equipmentSkillObserver.SkillFromEquipments)
        {
            if (skill is IModifierSkill modifierSkill)
            {
                Debug.Log(skill);

                _playerStat += modifierSkill.GetBonusStat();
            }
        }
    }

    protected override void MinusHP(int damage)
    {
        MinusStatModifier minusStatModifier = new MinusStatModifier();

        _playerStat.ModifyStat(StatComponentNameConstant.Health, minusStatModifier, damage);
    }

    protected override void InvokeUpdateHPBarEvent(float prevHp)
    {
        IStatComponent health = _playerStat.GetStat(StatComponentNameConstant.Health);

        setPlayerHpEvent?.Invoke(health.Value, health.BaseValue);
    }
}
