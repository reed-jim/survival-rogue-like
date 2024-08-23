using UnityEngine;
using static CustomDelegate;

public class CharacterCollectibleSpawner : MonoBehaviour
{
    [Header("CUSTOMIZE")]
    [SerializeField] private int numberCollectibleSpawn;

    #region ACTION
    public static GetICollectibleAction getICollectibleEvent;
    #endregion

    private void Awake()
    {
        Enemy.enemyDieEvent += SpawnExperienceShard;
    }

    private void OnDestroy()
    {
        Enemy.enemyDieEvent -= SpawnExperienceShard;
    }

    private void SpawnExperienceShard(int instanceId)
    {
        if (instanceId == gameObject.GetInstanceID())
        {
            Vector3 spawnPosition = transform.position;
            for (int i = 0; i < numberCollectibleSpawn; i++)
            {
                if (i > 0)
                {
                    if (i % 2 == 0)
                    {
                        spawnPosition.x += 1.5f;
                    }
                    else
                    {
                        spawnPosition.z += 1.5f;
                    }
                }

                ICollectible collectible = getICollectibleEvent?.Invoke();

                collectible?.Spawn(spawnPosition);
            }
        }
    }
}
