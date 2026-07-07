using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class GameLauncher : MonoBehaviour
{
    private async void Start()
    {

        // NetworkRunnerを生成してゲーム（部屋）を開始
        var runner = gameObject.AddComponent<NetworkRunner>();
        await runner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Shared, // 共有モード
            SessionName = "Room2vs2",   // 部屋名
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

}