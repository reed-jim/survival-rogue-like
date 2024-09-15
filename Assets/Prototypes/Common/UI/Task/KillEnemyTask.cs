using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pascal Case", menuName = "ScriptableObject/RPG/KillEnemyTask")]
public class KillEnemyTask : BaseTask
{
    [SerializeField] private int currentEnemyKilled;
    [SerializeField] private int requiredNumber;

    public override float GetCurrentProgress()
    {
        return (float)currentEnemyKilled / requiredNumber;
    }
}
