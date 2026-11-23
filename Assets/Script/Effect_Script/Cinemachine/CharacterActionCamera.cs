using UnityEngine;

//キャラクターごとのカメラ情報
[System.Serializable]
public class CharacterActionCamera
{
    [Header("キャラクターID")]
    public int characterID;

    [Header("アクション一覧")]
    public ActionCameraData[] actions;
}