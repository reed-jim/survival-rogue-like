using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [Header("MODULE - RANGED")]
    [SerializeField] private RangedAttack rangedAttack;

    // protected override void Attack()
    // {
    //     rangedAttack.Attack(player.transform, transform);

    //     _tweens.Add(Tween.Delay(5f).OnComplete(() =>
    //     {
    //         _state = CharacterState.IDLE;
    //     }));

    //     _state = CharacterState.ATTACK;
    // }
}
