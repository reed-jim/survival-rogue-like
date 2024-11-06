using System.Collections;
using System.Collections.Generic;
using Saferio.Util.SaferioTween;
using Unity.Netcode;
using UnityEngine;

public class NetworkedObjectSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerWeaponPrefab;
    [SerializeField] private JoystickController joystickController;
    [SerializeField] private RectTransform joystickControllerContainer;
    [SerializeField] private GameObject enemySpawner;

    private Vector3 _spawnPosition;
    private bool _isEnemySpawnerSpawned;

    private void Awake()
    {
        StartCoroutine(SpawnPlayers());
        // SaferioTween.DelayAsync(5, onCompletedAction: () => SpawnPlayers());
    }

    private void Spawn()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            GameObject player = Instantiate(playerPrefab);

            player.GetComponent<NetworkObject>().Spawn();

            Instantiate(enemySpawner).GetComponent<NetworkObject>().Spawn();
        }
    }

    private IEnumerator SpawnPlayers()
    {
        yield return new WaitForSeconds(5);

        yield return new WaitUntil(() => IsSpawned);

        if (NetworkManager.Singleton.IsServer)
        {
            foreach (var clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                GameObject player = Instantiate(playerPrefab);

                NetworkObject playerNetworkObject = player.GetComponent<NetworkObject>();

                playerNetworkObject.Spawn();
                playerNetworkObject.ChangeOwnership(clientId);

                GameObject playerWeapon = Instantiate(playerWeaponPrefab);

                NetworkObject playerWeaponNetworkObject = playerWeapon.GetComponent<NetworkObject>();

                playerWeaponNetworkObject.Spawn();
                playerWeaponNetworkObject.ChangeOwnership(clientId);

                playerWeaponNetworkObject.TrySetParent(playerNetworkObject);

                playerWeaponNetworkObject.transform.localPosition = new Vector3(0, 5, 0);
                playerWeaponNetworkObject.transform.localScale = new Vector3(5, 5, 50);

                player.GetComponent<PlayerAttack>().SwordCollider = playerWeapon.GetComponent<Collider>();
                player.GetComponent<PlayerAttack>().FakeWhirlwindAttackRigidBody = playerWeapon.GetComponent<Rigidbody>();
                playerWeapon.GetComponent<MeleeWeapon>().WeaponHolder = player;

                SetupPlayerAttackAndWeaponRpc(playerNetworkObject.NetworkObjectId, playerWeaponNetworkObject.NetworkObjectId);

                player.transform.position = _spawnPosition;

                _spawnPosition += new Vector3(4, 0, 0);

                SpawnJoystickController(clientId);
            }

            Instantiate(enemySpawner).GetComponent<NetworkObject>().Spawn();
        }
    }

    [Rpc(SendTo.NotServer)]
    private void SetupPlayerAttackAndWeaponRpc(ulong playerNetworkObjectId, ulong playerWeaponNetworkObjectId)
    {
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(playerNetworkObjectId, out NetworkObject player))
        {
            if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(playerWeaponNetworkObjectId, out NetworkObject playerWeapon))
            {
                player.GetComponent<PlayerAttack>().SwordCollider = playerWeapon.GetComponent<Collider>();
                player.GetComponent<PlayerAttack>().FakeWhirlwindAttackRigidBody = playerWeapon.GetComponent<Rigidbody>();
                playerWeapon.GetComponent<MeleeWeapon>().WeaponHolder = player.gameObject;
            }
        }
    }

    private void SpawnJoystickController(ulong clientId)
    {
        GameObject spawnedJoystickController = Instantiate(joystickController.gameObject, joystickControllerContainer);

        NetworkObject networkJoystickController = spawnedJoystickController.GetComponent<NetworkObject>();

        networkJoystickController.SpawnAsPlayerObject(clientId);

        networkJoystickController.TrySetParent(joystickControllerContainer);
    }
}
