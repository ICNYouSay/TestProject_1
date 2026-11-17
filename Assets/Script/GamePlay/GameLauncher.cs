using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using UnityEngine;

public class GameLauncher : MonoBehaviour
{
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
		if(Input.GetKeyDown(KeyCode.R))
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
        JoinOrCreateRoom($"Room_{Random.Range(1000, 9999)}");
    }
}