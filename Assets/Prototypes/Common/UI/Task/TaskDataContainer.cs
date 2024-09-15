using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pascal Case", menuName = "ScriptableObject/RPG/TaskDataContainer")]
public class TaskDataContainer : ScriptableObject
{
    [SerializeField] private BaseTask[] items;

    public BaseTask[] Items => items;
}
