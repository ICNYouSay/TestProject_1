using UnityEngine;
using System.Collections.Generic;

public class PortalSystem : MonoBehaviour
{
    [Header("スポーン設定")]
    public GameObject enemyPrefab;    // 召喚する敵のプレハブ
    public int waveCount = 5;         // 一度に召喚する数
    public Transform spawnPoint;      // 出現させる中心位置
    public float respawnTime = 10.0f; // 浄化後に敵が再召喚されるまでの待機時間

    [Header("ポータルの切り替え")]
    public GameObject purplePortalObj; // 浄化前の紫ポータル（最初ON）
    public GameObject bluePortalObj;   // 浄化後の水色ポータル（最初OFF）

    private List<GameObject> activeEnemies = new List<GameObject>(); // 現在出現中の敵リスト
    private bool isPurified = false; // 今浄化されている状態かどうかのフラグ

    void Start()
    {
        // ゲーム開始時に水色ポータルを強制的に非表示にする
        if (bluePortalObj != null) bluePortalObj.SetActive(false);
        if (purplePortalObj != null) purplePortalObj.SetActive(true);

        // ゲーム開始時に敵を召喚する
        SpawnWave();
    }

    void Update()
    {
        // 浄化中なら敵の監視は不要なので終了
        if (isPurified) return;

        // デバッグ用（Pキーを押すと敵を強制全滅)
        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach (GameObject enemy in activeEnemies)
            {
                // 敵を破壊する
                if (enemy != null) Destroy(enemy); 
            }
        }
        // ------------------------------------------------

        // リストの中身を整理（nullになった＝倒された敵を掃除する）
        activeEnemies.RemoveAll(enemy => enemy == null);

        // 敵が全滅したら浄化処理へ
        if (activeEnemies.Count == 0)
        {
            PurifyPortal();
        }

        // 浄化中なら敵の監視は不要なので終了
        if (isPurified) return;

    }

    // 敵を円形に召喚するメソッド
    void SpawnWave()
    {
        // エラーチェック
        if (spawnPoint == null) Debug.LogError("SpawnPointが設定されてないよ！");
        if (enemyPrefab == null) Debug.LogError("EnemyPrefabが設定されてないよ！");

        for (int i = 0; i < waveCount; i++)
        {
            // 5体なら72度ずつずらして円を描くように計算
            float angle = i * (360f / waveCount);

            // 半径6fの円周上の位置を計算
            Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad)) * 6f;

            // 敵を出現させる
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position + offset, spawnPoint.rotation);
            activeEnemies.Add(newEnemy);
        }
    }

    // ポータルを浄化する処理
    void PurifyPortal()
    {
        isPurified = true;

        // 念のため、nullチェックを強化
        if (purplePortalObj != null)
        {
            purplePortalObj.SetActive(false);
            Debug.Log("紫を消した");
        }
        else
        {
            Debug.LogError("紫のモデルがセットされてないよ！");
        }

        if (bluePortalObj != null)
        {
            bluePortalObj.SetActive(true);
            Debug.Log("水色を出した");
        }

        Invoke("ResetPortal", respawnTime);
    }

    // ポータルを元に戻して敵を再召喚する処理
    void ResetPortal()
    {
        isPurified = false;

        // モデルを元に戻す（水色を消して、紫を表示）
        if (purplePortalObj != null) purplePortalObj.SetActive(true);
        if (bluePortalObj != null) bluePortalObj.SetActive(false);

        // 敵を再召喚
        SpawnWave();
    }
}