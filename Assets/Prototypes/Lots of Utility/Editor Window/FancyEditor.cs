#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Saferio.Util;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.IO;
using System.Linq;

public class FancyEditor : EditorWindow
{
    private List<string> classNames = new List<string>();
    private Dictionary<string, List<string>> dependencies = new Dictionary<string, List<string>>();

    #region COLOR
    string titleColor = "F3FEB8";
    string headerColor = "FFDE4D";
    string textColor = "FFB22C";
    string backgroundColor = "FF4C4C";

    string buttonNormalBackgroundColor = "A0FFB4";
    string buttonHoverBackgroundColor = "C8FFB4";
    #endregion

    public bool isShowCustomization;
    public Font font;
    public int fontSize;

    #region TEXTURE
    public Texture2D buttonNormalBackground;
    public Texture2D buttonHoverBackground;
    #endregion

    [MenuItem("Tools/Saferio/Fancy")]
    private static void ShowWindow()
    {
        var window = GetWindow<FancyEditor>();
        window.titleContent = new GUIContent("Fancy");
        window.Show();
    }

    private void OnEnable()
    {
        // buttonNormalBackground = EditorUtil.CreateRoundedTexture(4, 4, 16, EditorUtil.GetColorFromHex(buttonNormalBackgroundColor), referenceTexture);
        // buttonHoverBackground = EditorUtil.CreateRoundedTexture(4, 4, 16, EditorUtil.GetColorFromHex(buttonHoverBackgroundColor), referenceTexture);

        // buttonNormalBackground = EditorUtil.CreateColorTexture(EditorUtil.GetColorFromHex(buttonNormalBackgroundColor));
        // buttonNormalBackground = EditorUtil.CreateColorTexture(EditorUtil.GetColorFromHex(buttonHoverBackgroundColor));
    }

    private void Update()
    {
        // Force update the editor
        try
        {
            if (mouseOverWindow.GetType() == typeof(FancyEditor))
            {
                Repaint();
            }
        }
        catch { }
    }

    private void OnGUI()
    {
        #region STYLE
        GUIStyle textStyle = GetTextStyle(backgroundColor, titleColor);
        GUIStyle titleStyle = GetTitleStyle(backgroundColor, titleColor);
        #endregion

        GUIStyle buttonStyle = GetButtonStyle
        (
            EditorUtil.GetColorFromHex(buttonNormalBackgroundColor), EditorUtil.GetColorFromHex(buttonHoverBackgroundColor)
        );

        #region PADDING
        float padding = 16;

        Rect area = new Rect(padding, padding, position.width - padding * 2f, position.height - padding * 2f);
        #endregion

        #region CUSTOMIZE AREA PADDING
        padding = 4;

        Rect customizeArea = new Rect(padding, padding, position.width - padding * 2f, position.height - padding * 2f);
        #endregion

        #region EDITOR
        GUILayout.BeginArea(area);

        GUILayout.Label("LEVEL EDITOR", titleStyle, GUILayout.MinHeight(40));

        EditorUtil.Row(
            rowContent: () =>
            {
                GUILayout.Label("Is Show Customization", textStyle);
                isShowCustomization = EditorGUILayout.Toggle(isShowCustomization);
            }
        );

        if (isShowCustomization)
        {
            CustomizationArea(customizeArea, textStyle, buttonNormalBackground);
        }

        if (GUILayout.Button("Get all scripts", buttonStyle))
        {
            LoadDependencies();
        }

        GUILayout.EndArea();
        #endregion
    }

    private void CustomizationArea(Rect customizeArea, GUIStyle textStyle, Texture2D referenceTexture)
    {
        int padding = 16;

        GUIStyle paddedStyle = new GUIStyle(GUI.skin.box)
        {
            padding = new RectOffset(padding, padding, padding, padding),
            normal = { background = EditorUtil.CreateColorTexture(new Color(0, 0, 0, 0f)) }
        };

        GUILayout.BeginVertical(paddedStyle);

        EditorUtil.Row(
            rowContent: () =>
            {
                GUILayout.Label("Font", textStyle);
                font = (Font)EditorGUILayout.ObjectField(font, typeof(Font));
            }
        );

        EditorUtil.Row(
            rowContent: () =>
            {
                GUILayout.Label("Font Size", textStyle);
                fontSize = EditorGUILayout.IntField(fontSize);
            }
        );

        EditorUtil.Row(
            rowContent: () =>
            {
                GUILayout.Label("Button Normal Background", textStyle);
                buttonNormalBackground = (Texture2D)EditorGUILayout.ObjectField(buttonNormalBackground, typeof(Texture2D));
            }
        );

        EditorUtil.Row(
            rowContent: () =>
            {
                GUILayout.Label("Button Hover Background", textStyle);
                buttonHoverBackground = (Texture2D)EditorGUILayout.ObjectField(buttonHoverBackground, typeof(Texture2D));
            }
        );

        GUILayout.EndVertical();
    }

    private void LoadDependencies()
    {
        classNames.Clear();
        dependencies.Clear();

        string scriptsFolder = "Assets/_Root/ECS Learning/Prototypes/Lots of Utility";
        string[] scriptFiles = Directory.GetFiles(scriptsFolder, "*.cs", SearchOption.AllDirectories);

        DebugUtil.ClearLogWithNotification();

        foreach (var scriptFile in scriptFiles)
        {
            string className = Path.GetFileNameWithoutExtension(scriptFile);
            Type type = FindTypeByName(className);

            if (type != typeof(SampleEventPublisher))
            {
                continue;
            }

            var events = type.GetEvents(BindingFlags.Public | BindingFlags.Static);

            foreach (var eventInfo in events)
            {
                Debug.Log("Found event: " + eventInfo.Name);

                // Get the invocation list
                var invocationList = GetStaticInvocationList(eventInfo);
                foreach (var handler in invocationList)
                {
                    Debug.Log("Subscribed method: " + handler.Method.Name);
                }
            }
        }
    }

    private EventInfo[] GetPublicStaticEvents(Type type)
    {
        return type.GetEvents(BindingFlags.Public | BindingFlags.Static)
                   //   .Where(field => field.FieldType.IsSubclassOf(typeof(MulticastDelegate)))
                   .ToArray();
    }

    private Delegate[] GetStaticInvocationList(EventInfo eventInfo)
    {
        // Use reflection to get the field associated with the event
        var fieldInfo = eventInfo.DeclaringType.GetField(eventInfo.Name, BindingFlags.Public | BindingFlags.Static);

        if (fieldInfo != null)
        {
            // Get the delegate from the field
            var eventDelegate = (Delegate)fieldInfo.GetValue(null);
            return eventDelegate?.GetInvocationList();
        }

        return Array.Empty<Delegate>();
    }

    private List<string> GetDependencies(Type type)
    {
        List<string> dependenciesList = new List<string>();
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

        foreach (var field in fields)
        {
            if (classNames.Contains(field.FieldType.Name))
            {
                dependenciesList.Add(field.FieldType.Name);
            }
        }

        foreach (var property in properties)
        {
            if (classNames.Contains(property.PropertyType.Name))
            {
                dependenciesList.Add(property.PropertyType.Name);

                DebugUtil.Log(type + " / " + type.Name, LogImportance.HIGH);
            }
        }

        return dependenciesList.Distinct().ToList();
    }

    private Type FindTypeByName(string className)
    {
        var assemblyTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.Name == className);

        return assemblyTypes.FirstOrDefault();
    }

    private GUIStyle GetTextStyle(string backgroundColor, string titleColor)
    {
        GUIStyle textSyle = new GUIStyle(GUI.skin.label);

        if (font != null)
        {
            textSyle.font = font;
        }

        if (fontSize > 0)
        {
            textSyle.fontSize = fontSize;
        }

        return textSyle;
    }

    private GUIStyle GetTitleStyle(string backgroundColor, string titleColor)
    {
        GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel);

        titleStyle.normal.textColor = EditorUtil.GetColorFromHex(titleColor);
        if (font != null)
        {
            titleStyle.font = font;
        }
        titleStyle.fontSize = 16;
        titleStyle.alignment = TextAnchor.MiddleCenter;

        return titleStyle;
    }

    private GUIStyle GetButtonStyle(Color normalTextColor, Color hoverTextColor)
    {
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);

        // if (buttonNormalBackground != null)
        // {
        //     buttonStyle.normal.background = buttonNormalBackground;
        // }

        // if (buttonHoverBackground != null)
        // {
        //     buttonStyle.hover.background = buttonHoverBackground;
        // }

        if (font != null)
        {
            buttonStyle.font = font;
        }

        buttonStyle.normal.textColor = normalTextColor;
        buttonStyle.hover.textColor = hoverTextColor;

        buttonStyle.alignment = TextAnchor.MiddleCenter;
        buttonStyle.padding = new RectOffset(8, 8, 8, 8);
        buttonStyle.border = new RectOffset(16, 16, 16, 16);

        return buttonStyle;
    }

    // private GUIStyle GetButtonStyle(Texture2D buttonNormalBackground, Texture2D buttonHoverBackground)
    // {
    //     GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);

    //     // if (buttonNormalBackground != null)
    //     // {
    //     //     buttonStyle.normal.background = buttonNormalBackground;
    //     // }

    //     // if (buttonHoverBackground != null)
    //     // {
    //     //     buttonStyle.hover.background = buttonHoverBackground;
    //     // }

    //     if (font != null)
    //     {
    //         buttonStyle.font = font;
    //     }

    //     // buttonStyle.normal.textColor = Color.red;
    //     // buttonStyle.hover.textColor = Color.black;

    //     buttonStyle.alignment = TextAnchor.MiddleCenter;
    //     buttonStyle.padding = new RectOffset(8, 8, 8, 8);
    //     buttonStyle.border = new RectOffset(16, 16, 16, 16);

    //     return buttonStyle;
    // }
}
#endif
