using System;
using Puzzle.Merge;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LevelSpawner : MonoBehaviour
{
    [Header("DEBUG")]
    [SerializeField] private bool isDebug;
    [SerializeField] private int level;

    #region ACTION
    public static event Action startLevelEvent;
    #endregion

    private void Awake()
    {
        WinLevelScreen.nextLevelEvent += NextLevel;

        if (isDebug)
        {
            LoadLevelAsset($"Level {level}");
        }
        else
        {
            LoadLevelAsset("Level 1");
        }
    }

    private void OnDestroy()
    {
        WinLevelScreen.nextLevelEvent -= NextLevel;
    }

    private void LoadLevelAsset(string level)
    {
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(level);

        using (var helper = new AddressablesHelper(handle))
        {
            handle.Completed += operationHandle =>
            {
                if (operationHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject prefab = operationHandle.Result;

                    Instantiate(prefab, transform);

                    startLevelEvent?.Invoke();
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

    private async void NextLevel()
    {
        DestroyImmediate(transform.GetChild(0).gameObject);

        LoadLevelAsset("Level 2");
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
