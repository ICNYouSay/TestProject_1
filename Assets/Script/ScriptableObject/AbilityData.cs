using UnityEngine;

[CreateAssetMenu(fileName = "AbilityData", menuName = "Scriptable Objects/AbilityData")]
public class AbilityData : ScriptableObject
{
    [Header("アビリティのステータス")]
    public string abilityName;  // あび名

    public float hp;                // HP
    public float atk;               // 攻撃力
    public float def;               // 防御力

    [Header("効果説明")]
    public string description;         // アビの説明

    enum AbilityType
    {
        singleTarget, // 単体攻撃
        multiTarget,  // 範囲攻撃
        selfBuff,      // 自己強化
        teamBuff,      // 味方強化
    }
    //[Header("アビリティの種類")]
    //public AbilityType abilityType; // アビリティの種類


}
