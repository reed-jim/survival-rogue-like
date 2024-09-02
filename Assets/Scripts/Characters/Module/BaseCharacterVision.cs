using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterVision : MonoBehaviour, ICharacterVision
{
    public virtual void FindEnemy()
    {
        throw new System.NotImplementedException();
    }
}
