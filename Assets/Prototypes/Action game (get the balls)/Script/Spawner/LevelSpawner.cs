using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LevelSpawner : MonoBehaviour
{
    private void Awake()
    {
        LoadLevelAsset();
    }

    private void LoadLevelAsset()
    {
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>("Level 1");

        using (var helper = new AddressablesHelper(handle))
        {
            handle.Completed += operationHandle =>
            {
                if (operationHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject prefab = operationHandle.Result;

                    Instantiate(prefab, transform);
                }
                else
                {

                }
            };
        }
    }

    private void Spawn(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject prefab = handle.Result;

            Instantiate(prefab, transform);
        }
        else
        {
            Debug.LogError("Failed to load prefab.");
        }
    }
}

public class AddressablesHelper : System.IDisposable
{
    private AsyncOperationHandle handle;

    public AddressablesHelper(AsyncOperationHandle handle)
    {
        this.handle = handle;
    }

    public void Dispose()
    {
        if (handle.IsValid())
        {
            Addressables.Release(handle);
        }
    }
}
