using UnityEngine;

[CreateAssetMenu(fileName = "skill", menuName = "ScriptableObjects/RPG/ModifierSkill")]
public class ModifierSkill : ScriptableObject
{
    [SerializeField] private CharacterStat modifierStat;
}
