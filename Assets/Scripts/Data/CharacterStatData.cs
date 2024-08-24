using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/RPG/CharacterStatData")]
public class CharacterStatData : ScriptableObject
{
    [SerializeField] private CharacterStat characterStat;

    public CharacterStat CharacterStat => characterStat;
}
