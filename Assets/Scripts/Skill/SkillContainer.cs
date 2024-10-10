using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill Container", menuName = "ScriptableObjects/RPG/SkillContainer")]
public class SkillContainer : ScriptableObject
{
    [SerializeField] private bool clickThisToAutoAssignSkillToThisContainer;
    [SerializeField] private List<BaseSkill> allSkills;

    public List<BaseSkill> AllSkills
    {
        get => allSkills; set => allSkills = value;
    }
    public int NumberOfSkill => allSkills.Count;

    private void OnValidate()
    {
        // if (allSkills != null)
        // {
        //     Debug.Log($"All skills assigned. Total: {allSkills.Count} skills");

        //     return;
        // }

        allSkills = new List<BaseSkill>();

        DirectoryInfo directoryInfo = new DirectoryInfo("Assets/Scriptables/Skill/All Skills");

        FileInfo[] folder = directoryInfo.GetFiles();

        List<string> paths = new List<string>();

        foreach (var item in folder)
        {
            string assetName = item.Name;

            if (assetName.Contains(".meta"))
            {

            }
            else
            {
                paths.Add($"Assets/Scriptables/Skill/All Skills/{assetName}");
            }
        }

        LoadAsset(paths);
    }

    private void LoadAsset(List<string> paths)
    {
#if UNITY_EDITOR
        foreach (var path in paths)
        {
            BaseSkill skill = (BaseSkill)AssetDatabase.LoadAssetAtPath(path, typeof(BaseSkill));

            if (skill != null)
            {
                allSkills.Add(skill);
            }
        }
#endif
    }
}
