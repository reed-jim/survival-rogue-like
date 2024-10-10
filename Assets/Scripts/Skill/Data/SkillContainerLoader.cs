using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SkillContainerLoader : MonoBehaviour
{
//     [SerializeField] private SkillContainer skillContainer;

//     private void Awake()
//     {
//         LoadSkills();
//     }

//     private void LoadSkills()
//     {
//         skillContainer.AllSkills = new List<ISkill>();

//         DirectoryInfo directoryInfo = new DirectoryInfo("Assets/Scriptables/Skill/All Skills");

//         FileInfo[] folder = directoryInfo.GetFiles();

//         List<string> paths = new List<string>();

//         foreach (var item in folder)
//         {
//             string assetName = item.Name;

//             if (assetName.Contains(".meta"))
//             {

//             }
//             else
//             {
//                 paths.Add($"Assets/Scriptables/Skill/All Skills/{assetName}");
//             }
//         }

//         LoadAsset(paths);
//     }

//     private async void LoadAsset(List<string> paths)
//     {
// #if UNITY_EDITOR
//         foreach (var path in paths)
//         {
//             ISkill skill = (ISkill)AssetDatabase.LoadAssetAtPath(path, typeof(ISkill));

//             skillContainer.AllSkills.Add(skill);
//         }

//         Debug.Log(skillContainer.AllSkills.Count);

//         EditorUtility.SetDirty(skillContainer);

//         AssetDatabase.SaveAssets();
// #endif
//     }
}
