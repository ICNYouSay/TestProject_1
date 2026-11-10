using Fusion;
using UnityEngine;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField] private CharacterData[] allCharacterData; // 全キャラのデータを入れておく
    [SerializeField] private AbilityData[] allAbilityData;     // 全アビのデータを入れておく
    [SerializeField] private Transform modelContainer;         // 見た目モデルを配置する親オブジェクト

    [Header("プレイヤーコントローラー")]
    [SerializeField] private PlayerController playerController;

    [Header("プレイヤーステータス")]
    [SerializeField] private PlayerStatus playerStatus;

    // どのキャラを選んだかを同期する変数
    [Networked] public int SelectedCharacterID { get; set; }

    // アビリティは固定4枠として別々に同期する
    [Networked] public int SelectedAbility0 { get; set; }
    [Networked] public int SelectedAbility1 { get; set; }
    [Networked] public int SelectedAbility2 { get; set; }
    [Networked] public int SelectedAbility3 { get; set; }

    public override void Spawned()
    {
        // 自分が操作するプレイヤーなら GameData から選んだ番号を入れる
        if (Object.HasInputAuthority)
        {
            SelectedCharacterID = GameData.SelectedCharacterIndex;

            // 足りない分は -1 にする
            var abilities = GameData.EquippedAbilities;
            SelectedAbility0 = abilities != null && abilities.Length > 0 ? abilities[0] : -1;
            SelectedAbility1 = abilities != null && abilities.Length > 1 ? abilities[1] : -1;
            SelectedAbility2 = abilities != null && abilities.Length > 2 ? abilities[2] : -1;
            SelectedAbility3 = abilities != null && abilities.Length > 3 ? abilities[3] : -1;
        }

        // キャラIDに応じて見た目とステータスを反映する
        ApplyCharacterSetup();
    }

    private void ApplyCharacterSetup()
    {
        if (allCharacterData == null || allCharacterData.Length == 0)
        {
            Debug.LogError("allCharacterData が未設定です");
            return;
        }

        if (SelectedCharacterID < 0 || SelectedCharacterID >= allCharacterData.Length)
        {
            Debug.LogError($"SelectedCharacterID が範囲外です: {SelectedCharacterID}");
            return;
        }

        CharacterData data = allCharacterData[SelectedCharacterID];

        // プレイヤーステータスを設定
        if (playerStatus != null)
        {
            playerStatus.Initialize(data);
        }
        else
        {
            Debug.LogWarning("playerStatus が未設定です");
        }

        // 見た目モデルを生成
        if (data.modelPrefab != null && modelContainer != null)
        {
            Instantiate(data.modelPrefab, modelContainer);
        }
        else
        {
            Debug.LogWarning("modelPrefab または modelContainer が未設定です");
        }

        // 回転補正を反映
        if (playerController != null)
        {
            playerController.modelRotationOffset = data.rotateoffset;
        }
        else
        {
            Debug.LogWarning("playerController が未設定です");
        }

        // カメラ設定
        if (Object.HasInputAuthority && CameraManager.Instance != null && playerController != null)
        {
            CameraManager.Instance.SetTarget(playerController.transform);
        }

        if (playerController != null)
        {
            playerController.transform.position = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        }
    }
}