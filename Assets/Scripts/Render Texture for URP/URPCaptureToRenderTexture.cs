using UnityEngine;

public class URPCaptureToRenderTexture : MonoBehaviour
{
    public Camera targetCamera; // Drag the camera here in the Inspector
    public RenderTexture renderTexture; // Drag your RenderTexture here

    void Start()
    {
        // Assign the RenderTexture to the Camera
        if (targetCamera != null && renderTexture != null)
        {
            targetCamera.targetTexture = renderTexture;
        }
        else
        {
            Debug.LogError("Camera or RenderTexture not assigned.");
        }
    }

    void OnDisable()
    {
        // Release the RenderTexture when the script is disabled
        if (targetCamera != null)
        {
            targetCamera.targetTexture = null; // Clear the target texture to stop rendering to it
        }
    }
}
