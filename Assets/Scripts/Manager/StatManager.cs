using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    [SerializeField] private PlayerStat playerStat;

    public static event Action<float> updateExpProgressBarEvent;

    private void Awake()
    {
        Enemy.enemyDieEvent += EarnPlayerExpKillingEnemy;
    }

    private void OnDestroy()
    {
        Enemy.enemyDieEvent -= EarnPlayerExpKillingEnemy;
    }

    private void EarnPlayerExpKillingEnemy(int enemyLevel)
    {
        playerStat.EarnExp(playerStat.GetExpFromKillEnemy(enemyLevel));

        updateExpProgressBarEvent?.Invoke(playerStat.EXP);
    }
}
