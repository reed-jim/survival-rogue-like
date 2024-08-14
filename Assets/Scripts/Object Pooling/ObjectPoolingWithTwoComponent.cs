using UnityEngine;

public class ObjectPoolingWithTwoComponent<T1, T2> : ObjectPoolingBase
{
    [Header("COMPONENT")]
    private T1[] _typeOneComponents;
    private T2[] _typeTwoComponents;

    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < poolSize; i++)
        {
            _typeOneComponents[i] = _poolObjects[i].GetComponent<T1>();
            _typeTwoComponents[i] = _poolObjects[i].GetComponent<T2>();
        }
    }

    protected T1 GetTypeOneComponentFromPool()
    {
        return _typeOneComponents[CurrentPoolObjectIndex];
    }

    protected T2 GetTypeTwoComponentFromPool()
    {
        return _typeTwoComponents[CurrentPoolObjectIndex];
    }
}
