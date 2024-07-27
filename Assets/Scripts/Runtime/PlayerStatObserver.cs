using UnityEngine;

[CreateAssetMenu(fileName = "player_stat_observer", menuName = "ScriptableObjects/PlayerStatObserver")]
public class PlayerStatObserver : ScriptableObject
{
    [SerializeField] private PlayerStat playerStat;

    public PlayerStat PlayerStat
    {
        get => playerStat;
        set => playerStat = value;
    }
}
