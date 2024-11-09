using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ReedJim.RPG.Stat;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment Skill Observer", menuName = "ScriptableObjects/RPG/EquipmentSkillObserver")]
public class EquipmentSkillObserver : ScriptableObject
{
    [SerializeField] private List<OwnedEquipmentData> equippedItemDatum;
    [SerializeField] private List<OwnedEquipmentData> ownedItemDatum;
    private List<BaseSkill> _skillFromEquipments;

    [SerializeField] private EquipmentSlotDataContainer equipmentSlotDataContainer;
    [SerializeField] private SkillContainer skillContainer;

    public List<OwnedEquipmentData> OwnedItemDatum
    {
        get => ownedItemDatum;
        set => ownedItemDatum = value;
    }

    // public List<OwnedEquipmentData> EquippedItemDatum
    // {
    //     get => LoadEquippedItems();
    //     // set => equippedItemDatum = value;
    // }

    public List<BaseSkill> SkillFromEquipments
    {
        get => _skillFromEquipments; set => _skillFromEquipments = value;
    }

    #region ACTION
    public static event Action<string, float> updateCharacterStatDisplayEvent;
    #endregion

    public void AddEquippedItem(OwnedEquipmentData ownedEquipmentData)
    {
        if (equippedItemDatum == null)
        {
            equippedItemDatum = new List<OwnedEquipmentData>();
        }

        equippedItemDatum.Add(ownedEquipmentData);

        _skillFromEquipments = equippedItemDatum.Select(e => e.GetSkill(skillContainer)).ToList();

        SaveEquippedItems();

        UpdateCharacterStat();
    }

    public void AddOwnedItem(OwnedEquipmentData ownedEquipmentData)
    {
        if (ownedItemDatum == null)
        {
            ownedItemDatum = new List<OwnedEquipmentData>();
        }

        ownedItemDatum.Add(ownedEquipmentData);

        _skillFromEquipments = ownedItemDatum.Select(e => e.GetSkill(skillContainer)).ToList();

        SaveEquippedItems();

        UpdateCharacterStat();
    }

    // public List<EquipmentData> LoadEquippedItems()
    // {
    //     int[] indexes = DataUtility.Load<int[]>(Constants.STAT_DATA_FILE_NAME, Constants.EQUIPPED_ITEM_INDEXES_IN_CONTAINER_DATA, null);

    //     List<EquipmentData> equippedItems = new List<EquipmentData>();

    //     foreach (var index in indexes)
    //     {
    //         equippedItems.Add(equipmentSlotDataContainer.Items[index]);
    //     }

    //     return equippedItems;
    // }

    public List<OwnedEquipmentData> LoadOwnedItems()
    {
        ownedItemDatum = DataUtility.Load<List<OwnedEquipmentData>>(Constants.STAT_DATA_FILE_NAME, Constants.OWNED_EQUIPMENTS, null);

        return ownedItemDatum;
    }

    private void SaveEquippedItems()
    {
        DataUtility.Save(Constants.STAT_DATA_FILE_NAME, Constants.OWNED_EQUIPMENTS, ownedItemDatum.ToArray());
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
