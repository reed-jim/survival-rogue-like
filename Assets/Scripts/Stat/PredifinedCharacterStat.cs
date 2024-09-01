using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReedJim.RPG.Stat
{
    [CreateAssetMenu(menuName = "ScriptableObjects/RPG/PredifinedCharacterStat")]
    public class PredifinedCharacterStat : ScriptableObject
    {
        [SerializeField] private SerializableStatComponent[] statComponents;

        public CharacterStat GetBaseCharacterStat()
        {
            CharacterStat baseStat = new CharacterStat();

            foreach (var item in statComponents)
            {
                baseStat.SetStatBaseValue(item.Key, item.BaseStatComponent.BaseValue);
            }

            return baseStat;
        }
    }
}
