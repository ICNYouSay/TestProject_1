using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class CharacterData : ScriptableObject
{
    public string characterName;  // キャラ名
    public GameObject modelPrefab;// モデル

    public int maxCost;           // コスト制限
    public int atk;               // 攻撃力
    public int def;               // 防御力
}
