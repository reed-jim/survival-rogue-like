using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pascal Case", menuName = "ScriptableObject/RPG/Task")]
public class BaseTask : ScriptableObject, ITask
{
    public virtual float GetCurrentProgress()
    {
        throw new System.NotImplementedException();
    }
}
