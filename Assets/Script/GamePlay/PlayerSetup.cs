using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField] private CharacterData[] allCharacterData; // 全キャラのデータを入れておく
    [SerializeField] private AbilityData[] allAbilityData; // 全アビのデータを入れておく
    [SerializeField] private Transform modelContainer;        // 見た目モデルを配置する親オブジェクト

    [Header("プレイヤーコントローラー")]
    [SerializeField]
    private PlayerController playerController;

    // どのキャラを選んだかを同期する変数
    [Networked] public int SelectedCharacterID { get; set; }

    public Button EquippedAbilities;

    //プレイヤーステータス
    [SerializeField]
    private PlayerStatus playerStatus;

    public override void Spawned()
    {
        // 自分が操作するプレイヤーならGameDataから選んだ番号を入れる
        if (Object.HasInputAuthority)
        {
            SelectedCharacterID = GameData.SelectedCharacterIndex;

            EquippedAbility0 = GameData.EquippedAbilities[0];
            EquippedAbility1 = GameData.EquippedAbilities[1];
            EquippedAbility2 = GameData.EquippedAbilities[2];
            EquippedAbility3 = GameData.EquippedAbilities[3];
            EquippedAbility4 = GameData.EquippedAbilities[4];
        }

        // 部屋に入った瞬間（またはIDが決まった瞬間）に見た目を生成
        CharacterData data = allCharacterData[SelectedCharacterID];

        //プレイヤーステータスを設定
        playerStatus.Initialize(data);

        // 元々ある見た目を消して、新しい見た目モデルを子要素として生成
        Instantiate(data.modelPrefab, modelContainer);

        // ステータス（コスト制限など）もこのデータから反映する
        playerController.modelRotationOffset = data.rotateoffset;

        Debug.Log($"CameraManager = {CameraManager.Instance}");
        Debug.Log($"PlayerController = {playerController}");
        Debug.Log($"HasInputAuthority = {Object.HasInputAuthority}");

        //自分が走査するプレイヤーだけカメラを設定
        if (Object.HasInputAuthority)
        {
            Debug.Log("自分のカメラを設定");
            CameraManager.Instance.SetTarget(playerController.transform);
        }

        playerController.transform.position = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
    }
}
