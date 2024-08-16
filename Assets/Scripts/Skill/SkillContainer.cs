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
    [SerializeField] private List<ISkill> allSkills;

    public List<ISkill> AllSkills => allSkills;
    public int NumberOfSkill => allSkills.Count;

    private void OnValidate()
    {
        // if (allSkills != null)
        // {
        //     Debug.Log($"All skills assigned. Total: {allSkills.Count} skills");

        //     return;
        // }

        allSkills = new List<ISkill>();

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

    private async void LoadAsset(List<string> paths)
    {
        foreach (var path in paths)
        {
            ISkill skill = (ISkill)AssetDatabase.LoadAssetAtPath(path, typeof(ISkill));

            allSkills.Add(skill);
        }
    }
}

public class Example : MonoBehaviour
{
    public static Example instance;

    void Start()
    {
        Example.instance = this;
    }
}
