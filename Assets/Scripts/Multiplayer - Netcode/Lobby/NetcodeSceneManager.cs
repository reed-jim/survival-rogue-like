using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetcodeSceneManager : NetworkBehaviour
{
    [SerializeField] private NetworkManager networkManager;

    private void Awake()
    {
        LobbyManagerUsingRelay.toGameplayEvent += LoadGameplayScene;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        LobbyManagerUsingRelay.toGameplayEvent -= LoadGameplayScene;
    }

    // public override void OnNetworkSpawn()
    // {
    //     if (IsServer && !string.IsNullOrEmpty("m_SceneName"))
    //     {
    //         var status = NetworkManager.SceneManager.LoadScene("m_SceneName", LoadSceneMode.Single);
    //         // var status = NetworkManager.SceneManager.LoadScene("m_SceneName", LoadSceneMode.Additive);

    //         if (status != SceneEventProgressStatus.Started)
    //         {

    //         }
    //     }
    // }

    public void LoadGameplayScene()
    {
        if (networkManager.IsServer)
        {
            var status = networkManager.SceneManager.LoadScene(Constants.GAMEPLAY_SCENE, LoadSceneMode.Single);

            if (status != SceneEventProgressStatus.Started)
            {

            }
        }
    }
}
