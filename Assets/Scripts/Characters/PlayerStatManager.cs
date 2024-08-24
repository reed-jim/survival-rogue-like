using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class PlayerStatManager : CharacterStatManager
{
    private PlayerStat _playerStat;

    public override CharacterStat Stat { get => _playerStat; }

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
        _playerStat = PlayerStat.Load();

        Tween.Delay(0.5f).OnComplete(() =>
        {
            addPlayerStatToListEvent?.Invoke(gameObject.GetInstanceID(), _playerStat);

            InvokeUpdateHPBarEvent(_playerStat.MaxHP);
        });
    }

    protected override void MinusHP(int damage)
    {
        _playerStat.HP -= damage;
    }

    protected override void InvokeUpdateHPBarEvent(float prevHp)
    {
        setPlayerHpEvent?.Invoke(Stat.HP, Stat.MaxHP);
    }
}
