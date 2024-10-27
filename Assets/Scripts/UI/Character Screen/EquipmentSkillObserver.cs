using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ReedJim.RPG.Stat;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment Skill Observer", menuName = "ScriptableObjects/RPG/EquipmentSkillObserver")]
public class EquipmentSkillObserver : ScriptableObject
{
    [SerializeField] private List<EquipmentSlotData> equippedItemDatum;
    [SerializeField] private List<BaseSkill> _skillFromEquipments;

    [SerializeField] private EquipmentSlotDataContainer equipmentSlotDataContainer;

    public List<EquipmentSlotData> EquippedItemDatum
    {
        get => LoadEquippedItems();
        // set => equippedItemDatum = value;
    }

    public List<BaseSkill> SkillFromEquipments
    {
        get => _skillFromEquipments; set => _skillFromEquipments = value;
    }

    #region ACTION
    public static event Action<string, float> updateCharacterStatDisplayEvent;
    #endregion

    public void Add(EquipmentSlotData equipmentData)
    {
        if (equippedItemDatum == null)
        {
            equippedItemDatum = new List<EquipmentSlotData>();
        }

        equippedItemDatum.Add(equipmentData);

        _skillFromEquipments = equippedItemDatum.Select(e => e.Skill).ToList();

        SaveEquippedItems();

        UpdateCharacterStat();
    }

    public List<EquipmentSlotData> LoadEquippedItems()
    {
        int[] indexes = DataUtility.Load<int[]>(Constants.STAT_DATA_FILE_NAME, Constants.EQUIPPED_ITEM_INDEXES_IN_CONTAINER_DATA, null);

        List<EquipmentSlotData> equippedItems = new List<EquipmentSlotData>();

        foreach (var index in indexes)
        {
            equippedItems.Add(equipmentSlotDataContainer.Items[index]);
        }

        return equippedItems;
    }

    private void SaveEquippedItems()
    {
        List<int> indexes = new List<int>();

        foreach (var item in equippedItemDatum)
        {
            for (int i = 0; i < equipmentSlotDataContainer.Items.Length; i++)
            {
                if (item == equipmentSlotDataContainer.Items[i])
                {
                    indexes.Add(i);

                    break;
                }
            }
        }

        DataUtility.Save(Constants.STAT_DATA_FILE_NAME, Constants.EQUIPPED_ITEM_INDEXES_IN_CONTAINER_DATA, indexes.ToArray());
    }

    // public void Add(BaseSkill skill)
    // {
    //     if (_skillFromEquipments == null)
    //     {
    //         _skillFromEquipments = new List<BaseSkill>();
    //     }

    //     _skillFromEquipments.Add(skill);

    //     UpdateCharacterStat();
    // }

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
