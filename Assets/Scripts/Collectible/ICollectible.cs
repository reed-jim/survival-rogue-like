using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollectible
{
    public void Spawn(Vector3 position);
    public void Collect();
}
