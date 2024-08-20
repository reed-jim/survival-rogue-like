using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
    public void Shoot(Transform target, Vector3 shotPosition, int attackInstanceId);
}
