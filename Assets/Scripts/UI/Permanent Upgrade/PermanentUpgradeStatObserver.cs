using System.Collections;
using System.Collections.Generic;
using ReedJim.RPG.Stat;
using UnityEngine;

[CreateAssetMenu(fileName = "Permanent Upgrade Stat Observer", menuName = "ScriptableObjects/RPG/PermanentUpgradeStatObserver")]
public class PermanentUpgradeStatObserver : ScriptableObject
{
    private CharacterStat _permanentUpgradeStat;

    public CharacterStat PermanentUpgradeStat
    {
        get => _permanentUpgradeStat; set => _permanentUpgradeStat = value;
    }
}
