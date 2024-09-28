#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System.IO;
using System.Diagnostics;

public class CaptureInGamePhotoUtil : EditorWindow
{
    public Camera renderCamera;
    public RenderTexture renderTexture;
    public SpriteRenderer spritePreviewer;

    private string _defaultDirectoryPath = "Assets/Captured/Photo";

    [MenuItem("Tools/Saferio/Canon Camera")]
    private static void ShowWindow()
    {
        var window = GetWindow<CaptureInGamePhotoUtil>();
        window.titleContent = new GUIContent("Canon Camera");
        window.Show();
    }

    private void OnGUI()
    {
        renderCamera = (Camera)EditorGUILayout.ObjectField("Render Texture Camera", renderCamera, typeof(Camera));
        renderTexture = (RenderTexture)EditorGUILayout.ObjectField("Render Texture", renderTexture, typeof(RenderTexture));
        spritePreviewer = (SpriteRenderer)EditorGUILayout.ObjectField("Sprite Previewer", spritePreviewer, typeof(SpriteRenderer));

        if (GUILayout.Button("Take Photo", GUILayout.MinHeight(40)))
        {
            CapturePhoto();
        }

        if (GUILayout.Button("Show in Explorer", GUILayout.MinHeight(40)))
        {
            OpenFolder(_defaultDirectoryPath);
        }
    }

    public void CapturePhoto()
    {
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = renderTexture;
        renderCamera.Render();

        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();

        RenderTexture.active = currentRT;

        Rect spriteRect = new Rect(0, 0, texture.width, texture.height);
        Sprite sprite = Sprite.Create(texture, spriteRect, new Vector2(0.5f, 0.5f));

        spritePreviewer.sprite = sprite;

        SaveTextureAsPNG(sprite.texture, _defaultDirectoryPath, "picture 0");
    }

    private void SaveTextureAsPNG(Texture2D texture, string directoryPath, string pictureName)
    {
        byte[] bytes = texture.EncodeToPNG();

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        string path = $"{directoryPath}/{pictureName}.png";

        System.IO.File.WriteAllBytes(path, bytes);

        DebugUtil.ClearLog();
        DebugUtil.DistinctLog($"Photo captured! Saved at {path}");
    }

    public void OpenFolder(string directoryPath)
    {
        string convertedDirectoryPath = directoryPath.Replace('/', '\\');

        if (System.IO.Directory.Exists(convertedDirectoryPath))
        {
            Process.Start("explorer.exe", convertedDirectoryPath); // Windows
        }
    }
}
#endif
