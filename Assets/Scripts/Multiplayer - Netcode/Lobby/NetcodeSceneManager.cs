using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetcodeSceneManager : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;

    private void Awake()
    {
        LobbyManagerUsingRelay.toGameplayEvent += LoadGameplayScene;
    }

    private void OnDestroy()
    {
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
        Debug.Log("load " + networkManager.IsServer);
        if (networkManager.IsServer)
        {
            var status = networkManager.SceneManager.LoadScene(Constants.GAMEPLAY_SCENE, LoadSceneMode.Single);

            if (status != SceneEventProgressStatus.Started)
            {

            }
        }
    }
}
