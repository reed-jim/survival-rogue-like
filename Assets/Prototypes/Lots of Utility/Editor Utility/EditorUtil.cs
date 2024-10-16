#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;

namespace Saferio.Util
{
    public static class EditorUtil
    {
        #region DISPLAY
        public static void Row(Action rowContent)
        {
            EditorGUILayout.BeginHorizontal();

            rowContent?.Invoke();

            EditorGUILayout.EndHorizontal();
        }
        #endregion

        #region WITH STYLE
        public static string StyledTextField(string label, string value, GUIStyle labelStyle, GUIStyle style, int height = 18)
        {
            GUILayout.Label(label, labelStyle, GUILayout.Height(height));

            return EditorGUILayout.TextField(value, style, GUILayout.Height(height));
        }

        public static float StyledNumberField(string label, float value, GUIStyle labelStyle, GUIStyle style, int height = 18)
        {
            float inputValue = 0;

            Row(() =>
            {
                GUILayout.Label(label, labelStyle, GUILayout.MinWidth(160), GUILayout.Height(height));

                inputValue = EditorGUILayout.FloatField(value, style, GUILayout.Height(height));
            });

            return inputValue;
        }

        public static UnityEngine.Object StyledObjectField(string label, UnityEngine.Object value, GUIStyle labelStyle, int height = 18)
        {
            UnityEngine.Object output = new UnityEngine.Object();

            Row(() =>
            {
                GUILayout.Label(label, labelStyle, GUILayout.MinWidth(160), GUILayout.Height(height));

                output = EditorGUILayout.ObjectField(value, typeof(UnityEngine.Object), GUILayout.Height(height));
            });

            return output;
        }
        #endregion

        public static Color GetColorFromHex(string hex)
        {
            if (hex.Length < 6)
            {
                throw new System.FormatException("Needs a string with a length of at least 6");
            }

            var r = hex.Substring(0, 2);
            var g = hex.Substring(2, 2);
            var b = hex.Substring(4, 2);

            string alpha;

            if (hex.Length >= 8)
            {
                alpha = hex.Substring(6, 2);
            }
            else
            {
                alpha = "FF";
            }

            return
            new Color
            (
                int.Parse(r, NumberStyles.HexNumber) / 255f,
                int.Parse(g, NumberStyles.HexNumber) / 255f,
                int.Parse(b, NumberStyles.HexNumber) / 255f,
                int.Parse(alpha, NumberStyles.HexNumber) / 255f
            );
        }

        public static Texture2D MakeTexture(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];

            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = col;
            }

            Texture2D result = new Texture2D(width, height);

            result.SetPixels(pix);

            result.Apply();

            return result;
        }

        public static Texture2D CreateColorTexture(Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }

        public static Texture2D MakeRoundedTexture(Color color)
        {
            int width = 400;
            int height = 200;
            Texture2D roundedTexture = new Texture2D(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float distanceX = x - width / 2;
                    float distanceY = y - height / 2;

                    if (distanceX * distanceX + distanceY * distanceY <= (width / 2) * (width / 2))
                    {
                        roundedTexture.SetPixel(x, y, color);
                    }
                    else
                    {
                        roundedTexture.SetPixel(x, y, new Color(0, 0, 0, 0));
                    }
                }
            }

            roundedTexture.Apply();
            return roundedTexture;
        }

        public static Texture2D CreateRoundedTexture(int width, int height, float cornerRadius, Color color, Texture2D referenceTexture)
        {
            Texture2D texture = new Texture2D(referenceTexture.width, referenceTexture.height);

            DebugUtil.DistinctLog(referenceTexture.width + "/" + referenceTexture.height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (referenceTexture.GetPixel(x, y).a == 0)
                    {
                        texture.SetPixel(x, y, new Color(0, 0, 0, 0));
                    }
                    else
                    {
                        texture.SetPixel(x, y, color);
                    }

                    // float distanceX = Mathf.Min(x, width - x);
                    // float distanceY = Mathf.Min(y, height - y);

                    // if (distanceX < cornerRadius && distanceY < cornerRadius)
                    // {
                    //     float distanceToCorner = Mathf.Sqrt((cornerRadius * cornerRadius) - (distanceX * distanceX) - (distanceY * distanceY));
                    //     if (distanceToCorner >= 0)
                    //     {
                    //         texture.SetPixel(x, y, color);
                    //     }
                    //     else
                    //     {
                    //         texture.SetPixel(x, y, new Color(0, 0, 0, 0));
                    //     }
                    // }
                    // else
                    // {
                    //     texture.SetPixel(x, y, color);
                    // }
                }
            }

            texture.Apply();
            return texture;
        }
    }
}
#endif
