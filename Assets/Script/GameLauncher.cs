using Fusion;
using UnityEngine;

public class GameLauncher : MonoBehaviour
{
    public GameObject playerPrefab; // プレイヤーのPrefabをInspectorで設定 
    private async void Start()
    {

        // NetworkRunnerを生成してゲーム（部屋）を開始
        var runner = gameObject.AddComponent<NetworkRunner>();
        runner.Spawn(playerPrefab, Vector3.zero, Quaternion.identity, runner.LocalPlayer);

        await runner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Shared, // 共有モード
            SessionName = "Room2vs2",   // 部屋名
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }
}