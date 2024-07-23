using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    private float _hp = 100;

    public int Level = 1;

    public float HP
    {
        get => _hp;
        set => _hp = value;
    }

    public void MinusHP(float value)
    {
        _hp -= value;
    }
}
