#if UNITY_EDITOR
using System.Reflection;
using UnityEngine;

public enum LogImportance
{
    LOW,
    NORMAL,
    HIGH
}

public static class DebugUtil
{
    public static void LogWithColor(string message, Color color)
    {
        Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{message}</color>");
    }

    public static void DistinctLog(object message)
    {
        Color textColor = new Color(1, 200f / 255, 160f / 255, 1);

        string text = $"KING-ADVENTURE-DEBUG: {message}";

        Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(textColor)}>{text}</color>");
    }

    public static void DistinctLog(string message)
    {
        Color textColor = new Color(1, 200f / 255, 160f / 255, 1);

        string text = $"KING-ADVENTURE-DEBUG: {message}";

        Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(textColor)}>{text}</color>");
    }

    public static void DistinctLog(string message, Color color)
    {
        string text = $"KING-ADVENTURE-DEBUG: {message}";

        Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{text}</color>");
    }

    public static void Log(string message, LogImportance logImportance)
    {
        float r = 1;
        float g = (220 - (int)logImportance * 60) / 255f;
        float b = (220 - (int)logImportance * 60) / 255f;

        Color textColor = new Color(r, g, b, 1);

        DistinctLog(message, textColor);
    }

    public static void ClearLog()
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));

        var type = assembly.GetType("UnityEditor.LogEntries");

        var method = type.GetMethod("Clear");

        method.Invoke(new object(), null);
    }

    public static void ClearLogWithNotification()
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));

        var type = assembly.GetType("UnityEditor.LogEntries");

        var method = type.GetMethod("Clear");

        method.Invoke(new object(), null);

        DistinctLog("Console Clear!");
    }
}
#endif
