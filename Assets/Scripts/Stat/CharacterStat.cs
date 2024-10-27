using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ReedJim.RPG.Stat
{
    [Serializable]
    public class SerializableStatComponent
    {
        [SerializeField] private string key;
        [SerializeField] private BaseStatComponent baseStatComponent;

        public string Key => key;
        public BaseStatComponent BaseStatComponent => baseStatComponent;
    }

    public class CharacterStat
    {
        [SerializeField] protected Dictionary<string, BaseStatComponent> statComponents;

        public Dictionary<string, BaseStatComponent> StatComponents => statComponents;

        public CharacterStat()
        {
            statComponents = new Dictionary<string, BaseStatComponent>();
        }

        public void AddStatComponent(string statComponentName, BaseStatComponent statComponent)
        {
            if (!statComponents.ContainsKey(statComponentName))
            {
                statComponents.Add(statComponentName, statComponent);
            }
        }

        public IStatComponent GetStat(string statComponentName)
        {
            return statComponents[statComponentName];
        }

        public string[] GetStatKeys()
        {
            return statComponents.Keys.ToArray();
        }

        public IStatComponent[] GetStats()
        {
            return statComponents.Values.ToArray();
        }

        public float GetStatValue(string statComponentName)
        {
            if (statComponents.ContainsKey(statComponentName))
            {
                return statComponents[statComponentName].Value;
            }
            else
            {
                return 0;
            }
        }

        public float GetStatBaseValue(string statComponentName)
        {
            return statComponents[statComponentName].BaseValue;
        }

        public void SetStatValue(string statComponentName, float amount)
        {
            statComponents[statComponentName].BaseValue = amount;
        }

        public void SetStatBaseValue(string statComponentName, float amount)
        {
            if (statComponents.ContainsKey(statComponentName))
            {
                statComponents[statComponentName].BaseValue = amount;
                statComponents[statComponentName].Value = amount;
            }
            else
            {
                BaseStatComponent baseStatComponent = new BaseStatComponent
                {
                    Value = amount,
                    BaseValue = amount
                };

                statComponents.Add(statComponentName, baseStatComponent);
            }
        }

        public void ResetStat(string statComponentName)
        {
            statComponents[statComponentName].Reset();
        }

        public void ModifyStat(string statComponentName, IStatModifier statModifier, float amount)
        {
            var stat = statComponents[statComponentName];

            statModifier.ModifyValue(stat, amount);
        }

        public void Save(string key)
        {
            DataUtility.Save(Constants.STAT_DATA_FILE_NAME, key, this);
        }

        public static CharacterStat Load(string key, CharacterStat baseStat)
        {
            return DataUtility.Load(Constants.STAT_DATA_FILE_NAME, key, baseStat);
        }
    }

    public class StatUsage
    {
        private CharacterStat characterStat;

        private void Awake()
        {
            characterStat.AddStatComponent(StatComponentNameConstant.Health, new BaseStatComponent());
            characterStat.AddStatComponent(StatComponentNameConstant.Damage, new BaseStatComponent());
            characterStat.AddStatComponent(StatComponentNameConstant.CriticalChance, new BaseStatComponent());
        }

        private void MinusHealth(float amount)
        {
            FlatStatModifier flatStatModifier = new FlatStatModifier();

            // Solution 1
            characterStat.ModifyStat(StatComponentNameConstant.Health, flatStatModifier, amount);

            // Solution 2
            flatStatModifier.ModifyValue(characterStat.GetStat(StatComponentNameConstant.Health), amount);
        }
    }
}
