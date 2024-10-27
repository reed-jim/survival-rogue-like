using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/RPG/Equipment/EquipmentVisualProvider")]
public class EquipmentVisualProvider : ScriptableObject
{
    [SerializeField] private List<Sprite> equipmentSprites;

    public List<Sprite> EquipmentSprites => equipmentSprites;

    // private void OnValidate()
    // {
    //     // if (equipmentSprites.Count > 0)
    //     // {
    //     //     return;
    //     // }

    //     equipmentSprites = new List<Sprite>();

    //     DirectoryInfo directoryInfo = new DirectoryInfo("Assets/3rd/Violet Theme Ui/White Icons");

    //     FileInfo[] folder = directoryInfo.GetFiles();

    //     List<string> paths = new List<string>();

    //     foreach (var item in folder)
    //     {
    //         string assetName = item.Name;

    //         if (assetName.Contains(".meta"))
    //         {

    //         }
    //         else
    //         {
    //             paths.Add($"Assets/3rd/Violet Theme Ui/White Icons/{assetName}");
    //         }
    //     }

    //     LoadAsset(paths);
    // }

    private void LoadAsset(List<string> paths)
    {
#if UNITY_EDITOR
        foreach (var path in paths)
        {
            Sprite sprite = (Sprite)AssetDatabase.LoadAssetAtPath(path, typeof(Sprite));

            if (sprite != null)
            {
                equipmentSprites.Add(sprite);
            }
        }
#endif
    }
}
