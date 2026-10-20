using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class CharacterData : ScriptableObject
{
    public string characterName;  // キャラ名
    public GameObject modelPrefab;// モデル
    public float rotateoffset;     // モデルの回転オフセット

    public float hp;                // HP
    public float atk;               // 攻撃力
    public float def;               // 防御力

    public string description;         // キャラの説明
}
