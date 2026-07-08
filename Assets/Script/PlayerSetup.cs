using Fusion;
using UnityEngine;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField] private CharacterData[] allCharacterData; // 全キャラのデータを入れておく
    [SerializeField] private Transform modelContainer;        // 見た目モデルを配置する親オブジェクト

    // どのキャラを選んだかを同期する変数
    [Networked] public int SelectedCharacterID { get; set; }
    public override void Spawned()
    {
        // 部屋に入った瞬間（またはIDが決まった瞬間）に見た目を生成
        CharacterData data = allCharacterData[SelectedCharacterID];

        // 元々ある見た目を消して、新しい見た目モデルを子要素として生成
        Instantiate(data.modelPrefab, modelContainer);

        // ステータス（コスト制限など）もこのデータから反映する
    }
}
