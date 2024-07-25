using System.IO;
using UnityEngine;

public static class DataUtility
{
    public static void Save(object data)
    {
        string filePath = Application.persistentDataPath + "/data.json";

        File.WriteAllText(filePath, JsonUtility.ToJson(data));
    }

    public static T Load<T>(T defaultValue)
    {
        string filePath = Application.persistentDataPath + "/data.json";

        if (File.Exists(filePath))
        {
            string data = File.ReadAllText(filePath);

            if (data != "")
            {
                return JsonUtility.FromJson<T>(data);
            }
            else
            {
                return defaultValue;
            }
        }
        else
        {
            return defaultValue;
        }
    }
}
