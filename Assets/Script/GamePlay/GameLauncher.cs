using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;


public class GameLauncher : MonoBehaviour, INetworkRunnerCallbacks
{
    [Header("ボスの参照")]
    [SerializeField] private BossEnemy bossEnemy; 　　　　// ボス
    private bool hasTransitioned = false;        　　　　 // 多重遷移の防止

    private NetworkRunner runner;
    // 取得した最新の部屋一覧を保持しておくリスト
    private List<SessionInfo> _cachedSessionList = new List<SessionInfo>();

    private async void Start()
	{

		// NetworkRunnerを生成してゲーム（部屋）を開始
		runner = gameObject.AddComponent<NetworkRunner>();

        var result = await runner.JoinSessionLobby(SessionLobby.Shared);

    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameObject[] data = GameObject.FindGameObjectsWithTag("Player");
            // プレイヤーデータを削除
            for (int i = 0; i < data.Length; i++)
            {
                Debug.Log("Destroy PlayerData");
                Destroy(data[i]);
            }

            runner.Shutdown();
            // リザルトシーンへ遷移(デバッグ用)
            UnityEngine.SceneManagement.SceneManager.LoadScene("ResultScene");
        }
    }

    // ボスから通知を受け取る専用の関数
    public void OnBossDefeated()
    {
        // 多重実行防止
        if (hasTransitioned) return;
        hasTransitioned = true;

        Debug.Log("GameLauncher：ボス撃破の通知を受信しました！リザルトへ移行します。");

        // ネットワークの終了処理
        if (runner != null)
        {
            runner.Shutdown();
        }

        // リザルトシーンへ遷移
        UnityEngine.SceneManagement.SceneManager.LoadScene("ResultScene");
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        _cachedSessionList = sessionList;
        JoinAvailableRoom();
    }

    public async void JoinOrCreateRoom(string roomName)
    {

        await runner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Shared, // 共有モード
            SessionName = roomName,   // 部屋名
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
            PlayerCount = 4,
            IsVisible = true
        });
    }

    public void JoinAvailableRoom()
    {
        foreach (var session in _cachedSessionList)
        {
            // 入室可能 かつ 定員割れしている部屋を探す
            if (session.IsOpen && session.PlayerCount < session.MaxPlayers)
            {
                JoinOrCreateRoom(session.Name);
                return;
            }
        }

        // 空いている部屋がなければ新しく作る
        JoinOrCreateRoom($"Room_{UnityEngine.Random.Range(1000, 9999)}");
    }

    // --- 以下、INetworkRunnerCallbacks の必須メソッド（未使用なら空でOK） ---
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, System.ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ReadOnlySpan<byte> data)
    {
        throw new NotImplementedException();
    }
}