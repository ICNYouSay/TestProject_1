using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NetworkConnectManager : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkObject playerPrefab; // プレイヤーのプレハブ

    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    // 誰かが部屋に参加したときに、全員の画面で呼び出される関数
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        // ① 参加した「player」が「自分（ローカルプレイヤー）」だった場合のみ実行
        if (player == runner.LocalPlayer)
        {
            Debug.Log("部屋への入室が完了しました。自分のキャラクターを生成します。");

            Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(-3, 3), 0, UnityEngine.Random.Range(-3, 3));

            // 自分のキャラをSpawn（第4引数にplayerを渡すことで、自分が操作権限[State Authority]を持つ）
            runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity, player);
        }

    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ReadOnlySpan<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }
}