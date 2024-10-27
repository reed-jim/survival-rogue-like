using System;
using System.Collections;
using System.Collections.Generic;
using ReedJim.RPG.Stat;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment Skill Observer", menuName = "ScriptableObjects/RPG/EquipmentSkillObserver")]
public class EquipmentSkillObserver : ScriptableObject
{
    [SerializeField] private List<BaseSkill> _skillFromEquipments;

    public List<BaseSkill> SkillFromEquipments
    {
        get => _skillFromEquipments; set => _skillFromEquipments = value;
    }

    #region ACTION
    public static event Action<string, float> updateCharacterStatDisplayEvent;
    #endregion

    public void Add(BaseSkill skill)
    {
        if (_skillFromEquipments == null)
        {
            _skillFromEquipments = new List<BaseSkill>();
        }

        _skillFromEquipments.Add(skill);

        UpdateCharacterStat();
    }

    private void UpdateCharacterStat()
    {
        CharacterStat characterStat = new CharacterStat();

        foreach (var skill in _skillFromEquipments)
        {
            try
            {
                IModifierSkill modifierSkill = (IModifierSkill)skill;

                FlatStatModifier flatStatModifier = new FlatStatModifier();

                foreach (var statComponent in modifierSkill.GetBonusStat().StatComponents)
                {
                    if (characterStat.StatComponents.ContainsKey(statComponent.Key))
                    {
                        characterStat.ModifyStat(statComponent.Key, flatStatModifier, statComponent.Value.BaseValue);
                    }
                    else
                    {
                        characterStat.AddStatComponent(statComponent.Key, statComponent.Value);
                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        foreach (var statComponent in characterStat.StatComponents)
        {
            updateCharacterStatDisplayEvent?.Invoke(statComponent.Key, statComponent.Value.BaseValue);
        }
    }
}
