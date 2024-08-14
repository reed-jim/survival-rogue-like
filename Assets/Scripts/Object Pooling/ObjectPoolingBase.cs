using UnityEngine;

public class ObjectPoolingBase : MonoBehaviour
{
    [Header("POOL PREFAB")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform poolContainer;

    [Header("CUSTOMIZE")]
    [SerializeField] protected int poolSize;

    [Header("POOL")]
    protected GameObject[] _poolObjects;

    [Header("MANAGEMENT")]
    private int _currentPoolObjectIndex;

    public int CurrentPoolObjectIndex
    {
        get => _currentPoolObjectIndex;
    }

    protected virtual void Awake()
    {
        _poolObjects = new GameObject[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            _poolObjects[i] = Instantiate(prefab, poolContainer);
        }
    }

    protected GameObject GetFromPool()
    {
        GameObject gameObjectToGet = _poolObjects[_currentPoolObjectIndex];

        ChangeIndex();

        return gameObjectToGet;
    }

    protected void ChangeIndex()
    {
        _currentPoolObjectIndex++;

        if (_currentPoolObjectIndex >= poolSize)
        {
            _currentPoolObjectIndex = 0;
        }
    }
}
