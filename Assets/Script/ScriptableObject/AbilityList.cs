using UnityEngine;

[CreateAssetMenu(fileName = "AbilityList", menuName = "Scriptable Objects/AbilityList")]
public class AbilityList : ScriptableObject
{
    [Header("アビリティの一覧")]
    public AbilityData[] abilities;
}
