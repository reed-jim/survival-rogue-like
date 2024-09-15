using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISubcriber
{
    public void RegisterEvent();

    public void UnregisterEvent();
}
