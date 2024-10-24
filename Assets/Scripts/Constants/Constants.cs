using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public static string PLAYER_TAG = "Player";
    public static string ENEMY_TAG = "Enemy";
    public static string PLAYER_BULLET_TAG = "Player Bullet";

    public static string STAT_DATA_FILE_NAME = "stat_data";

    public static string PERMANENT_UPGRADE_STAT = "permanent_upgrade_stat";

    #region ANIMATION
    public static int ANIMATION_MOVEMENT_STATE = 0;
    public static int ANIMATION_ATTACK_STATE = 1;
    public static int ANIMATION_DIE_STATE = 2;
    #endregion

    #region LAYER
    public static string DEAD_LAYER = "Dead";
    #endregion
}
