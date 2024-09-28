#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

public class ModelToSpriteTool : EditorWindow
{
    public Camera renderCamera;
    public RenderTexture renderTexture;
    public SpriteRenderer spritePreviewer;

    [MenuItem("Tools/Saferio/Model To Sprite Tool")]
    private static void ShowWindow()
    {
        var window = GetWindow<ModelToSpriteTool>();
        window.titleContent = new GUIContent("ModelToSpriteTool");
        window.Show();
    }

    private void OnGUI()
    {
        renderCamera = (Camera)EditorGUILayout.ObjectField("Render Texture Camera", renderCamera, typeof(Camera));
        renderTexture = (RenderTexture)EditorGUILayout.ObjectField("Render Texture", renderTexture, typeof(RenderTexture));
        spritePreviewer = (SpriteRenderer)EditorGUILayout.ObjectField("Sprite Previewer", spritePreviewer, typeof(SpriteRenderer));

        if (GUILayout.Button("Generate"))
        {
            CreateSpriteFromRenderTexture();
        }
    }

    public Sprite CreateSpriteFromRenderTexture()
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

        return sprite;
    }
}
#endif
