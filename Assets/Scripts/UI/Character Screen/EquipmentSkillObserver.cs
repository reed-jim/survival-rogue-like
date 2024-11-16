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

    #region CHARACTER STAT
    [Header("CHARACTER STAT")]
    [SerializeField] private PredifinedCharacterStat defaultCharacterStat;
    private CharacterStat _playerStat;
    #endregion

    #region PROPERTY
    public List<OwnedEquipmentData> OwnedItemDatum
    {
        get => ownedItemDatum;
        set => ownedItemDatum = value;
    }

    public List<OwnedEquipmentData> EquippedItemDatum
    {
        get => equippedItemDatum;
        set => equippedItemDatum = value;
    }

    public List<BaseSkill> SkillFromEquipments
    {
        get => _skillFromEquipments; set => _skillFromEquipments = value;
    }
    #endregion

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

        UpdateCharacterStat(_skillFromEquipments);
    }

    public void UnequippedItem(OwnedEquipmentData ownedEquipmentData)
    {
        foreach (var item in equippedItemDatum)
        {
            if (item == ownedEquipmentData)
            {
                equippedItemDatum.Remove(item);

                break;
            }
        }

        _skillFromEquipments = equippedItemDatum.Select(e => e.GetSkill(skillContainer)).ToList();

        SaveEquippedItems();

        UpdateCharacterStat(_skillFromEquipments);
    }

    public void AddOwnedItem(OwnedEquipmentData ownedEquipmentData)
    {
        if (ownedItemDatum == null)
        {
            ownedItemDatum = new List<OwnedEquipmentData>();
        }

        ownedItemDatum.Add(ownedEquipmentData);

        _skillFromEquipments = ownedItemDatum.Select(e => e.GetSkill(skillContainer)).ToList();

        SaveOwnedItems();
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

    public void Load()
    {
        _playerStat = DataUtility.Load(Constants.STAT_DATA_FILE_NAME, Constants.PLAYER_TAG, defaultCharacterStat.GetBaseCharacterStat());

        LoadOwnedItems();
        LoadEquippedItems();
    }

    public List<OwnedEquipmentData> LoadOwnedItems()
    {
        ownedItemDatum = DataUtility.Load<List<OwnedEquipmentData>>(Constants.STAT_DATA_FILE_NAME, Constants.OWNED_EQUIPMENTS, null);

        if (ownedItemDatum == null)
        {
            return null;
        }

        _skillFromEquipments = ownedItemDatum.Select(e => e.GetSkill(skillContainer)).ToList();

        UpdateCharacterStat(_skillFromEquipments);

        return ownedItemDatum;
    }

    public List<OwnedEquipmentData> LoadEquippedItems()
    {
        equippedItemDatum = DataUtility.Load<List<OwnedEquipmentData>>(Constants.STAT_DATA_FILE_NAME, Constants.EQUIPPED_EQUIPMENTS, null);

        if (equippedItemDatum == null)
        {
            return null;
        }

        _skillFromEquipments = equippedItemDatum.Select(e => e.GetSkill(skillContainer)).ToList();

        UpdateCharacterStat(_skillFromEquipments);

        return equippedItemDatum;
    }

    private void SaveOwnedItems()
    {
        DataUtility.Save(Constants.STAT_DATA_FILE_NAME, Constants.OWNED_EQUIPMENTS, ownedItemDatum.ToArray());
    }

    private void SaveEquippedItems()
    {
        DataUtility.Save(Constants.STAT_DATA_FILE_NAME, Constants.EQUIPPED_EQUIPMENTS, equippedItemDatum.ToArray());
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

    private void UpdateCharacterStat(List<BaseSkill> skillFromEquipments)
    {
        CharacterStat characterStat = _playerStat;

        foreach (var skill in skillFromEquipments)
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
