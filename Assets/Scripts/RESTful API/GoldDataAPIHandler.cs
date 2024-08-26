using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GoldDataAPIHandler : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(GETMethod());
    }

    private IEnumerator GETMethod()
    {
        using (UnityWebRequest request = UnityWebRequest.Get("https://www.goldapi.io/api/XAU/USD"))
        {
            request.SetRequestHeader("x-access-token", "goldapi-ao3fppsm0b11kxm-io");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Error: " + request.error);
            }
            else
            {
                Debug.Log("Received: " + request.downloadHandler.text);
            }
        }
    }
}
