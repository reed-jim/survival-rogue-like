using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    [SerializeField] private EquipmentSkillObserver equipmentSkillObserver;

    private void Awake()
    {
        equipmentSkillObserver.Load();
    }
}
