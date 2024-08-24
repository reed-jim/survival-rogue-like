using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveLoad
{
    public void Save(string key);
    public T Load<T>(string key, T defaultValue);
}
