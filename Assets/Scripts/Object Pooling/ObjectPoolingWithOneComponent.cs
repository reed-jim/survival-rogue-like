using UnityEngine;

public class ObjectPoolingWithOneComponent<T> : ObjectPoolingBase
{
    [Header("COMPONENT")]
    private T[] _components;

    protected override void Awake()
    {
        base.Awake();

        _components = new T[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            _components[i] = _poolObjects[i].GetComponent<T>();
        }
    }

    protected T GetComponentFromPool()
    {
        T component = _components[CurrentPoolObjectIndex];

        ChangeIndex();

        return component;
    }
}
