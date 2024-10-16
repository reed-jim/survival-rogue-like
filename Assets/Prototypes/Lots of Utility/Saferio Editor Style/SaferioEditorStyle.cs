#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Saferio.Editor.Style
{
    public static class SaferioEditorStyle
    {
        static Font font = AssetDatabase.LoadAssetAtPath<Font>("Assets/_Root/Scripts/Odeeo/Demo/Font/Roboto-Medium.ttf");
        static string textColor = "00FFAB";
        static string labelColor = "14C38E";
        static string headerColor = "E3FCBF";

        #region STYLE
        public static GUIStyle HeaderStyle()
        {
            GUIStyle headerStyle = new GUIStyle(EditorStyles.label);

            headerStyle.font = font;
            headerStyle.fontSize = 16;
            headerStyle.normal.textColor = ColorUtil.GetColorFromHex(headerColor);
            // headerStyle.alignment = TextAnchor.MiddleCenter;

            return headerStyle;
        }

        public static GUIStyle LabelStyle()
        {
            GUIStyle style = new GUIStyle(EditorStyles.label);

            style.font = font;
            style.normal.textColor = ColorUtil.GetColorFromHex(labelColor);
            style.alignment = TextAnchor.MiddleLeft;

            return style;
        }

        public static GUIStyle TextFieldStyle()
        {
            GUIStyle textFieldStyle = new GUIStyle(EditorStyles.numberField);

            textFieldStyle.font = font;
            textFieldStyle.normal.textColor = ColorUtil.GetColorFromHex(textColor);

            return textFieldStyle;
        }
        #endregion

        public static void HeaderWithSpace(string headerName, float space = 16)
        {
            EditorGUILayout.Space(space);

            GUILayout.Label(headerName, HeaderStyle());

            EditorGUILayout.Space(space);
        }
    }
}
#endif
