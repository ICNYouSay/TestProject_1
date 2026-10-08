using Effekseer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

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

    [Header("フェード設定")]
    public float fadeTime = 0.5f;      // フェード時間

    private List<GameObject> activeEnemies = new List<GameObject>(); // 現在出現中の敵リスト
    private bool isPurified = false;   // 今浄化されている状態かどうかのフラグ

    [Header("浄化プレハブ")]
    [SerializeField]
    private EffekseerEmitter PurificationPrefab;

    [Header("敵出現エフェクトプレハブ")]
    [SerializeField]
    private EffekseerEmitter EnemySpawnPrefab;

    [Header("エフェクト設定")]
    public float effectPlayTime = 0f;   // フェード開始から何秒後に再生するか

    // 紫ポータルのRenderer
    private Renderer[] purpleRenderer;

    // 青ポータルのRenderer
    private Renderer[] blueRenderer;

    //=========================
    // 初期化
    //=========================
    void Start()
    {
        //-------------------------
        // ポータルを表示
        //-------------------------
        purplePortalObj.SetActive(true);
        bluePortalObj.SetActive(true);

        //-------------------------
        // Renderer取得
        //-------------------------
        purpleRenderer =
            purplePortalObj.GetComponentsInChildren<Renderer>();

        blueRenderer =
            bluePortalObj.GetComponentsInChildren<Renderer>();

        //-------------------------
        // 最初は紫だけ表示
        //-------------------------
        SetAlpha(purpleRenderer, 1.0f);
        SetAlpha(blueRenderer, 0.0f);

        //------------------------
        // 最後に非表示
        //------------------------
        bluePortalObj.SetActive(false);

        //-------------------------
        // ゲーム開始時に敵を召喚
        //-------------------------
        SpawnWave();

    }

    //=========================
    // 更新
    //=========================
    void Update()
    {
        //-------------------------
        // 浄化中なら敵の監視は不要
        //-------------------------
        if (isPurified)
        {
            return;
        }

        //-------------------------
        // デバッグ用
        //-------------------------
        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach (GameObject enemy in activeEnemies)
            {
                if (enemy != null)
                {
                    Destroy(enemy);
                }
            }
        }

        //-------------------------
        // リストの掃除
        //-------------------------
        activeEnemies.RemoveAll(enemy => enemy == null);

        //-------------------------
        // 敵が全滅したら浄化
        //-------------------------
        if (activeEnemies.Count == 0)
        {
            PurifyPortal();
        }
    }

    //=========================
    // 敵を円形に召喚するメソッド
    //=========================
    void SpawnWave()
    {
        //-------------------------
        // エラーチェック
        //-------------------------
        if (spawnPoint == null)
        {
            Debug.LogError("SpawnPointが設定されてないよ！");
            return;
        }

        if (enemyPrefab == null)
        {
            Debug.LogError("EnemyPrefabが設定されてないよ！");
            return;
        }

        //-------------------------
        // 円形に敵を召喚
        //-------------------------
        for (int i = 0; i < waveCount; i++)
        {
            // 5体なら72度ずつずらして円を描くように計算
            float angle = i * (360f / waveCount);

            // 半径6fの円周上の位置を計算
            Vector3 offset =
                new Vector3(
                    Mathf.Cos(angle * Mathf.Deg2Rad),
                    0,
                    Mathf.Sin(angle * Mathf.Deg2Rad))
                * 6f;

            // 敵を出現させる
            GameObject newEnemy =
                Instantiate(
                    enemyPrefab,
                    spawnPoint.position + offset,
                    spawnPoint.rotation);

            // 敵出現エフェクトを再生
            if (EnemySpawnPrefab != null)
            {
                EffekseerEmitter effect =
                    Instantiate(
                        EnemySpawnPrefab,
                        newEnemy.transform.position,
                        newEnemy.transform.rotation);
                effect.Play();
            }

            // リストへ追加
            activeEnemies.Add(newEnemy);
        }
    }

    //=========================
    // ポータルを浄化する処理
    //=========================
    void PurifyPortal()
    {
        //-------------------------
        // 多重実行防止
        //-------------------------
        if (isPurified)
        {
            return;
        }

        //-------------------------
        // 浄化状態にする
        //-------------------------
        isPurified = true;

        Debug.Log("ポータル浄化開始");

        //-----------------
        //青ポータルを表示
        // ----------------
        bluePortalObj.SetActive(true);

        //-------------------------
        // フェード開始
        //-------------------------
        StartCoroutine(FadePortal());
    }

    //=========================
    // ポータルをフェードさせる
    //=========================
    IEnumerator FadePortal()
    {
        //-------------------------
        // フェード開始
        //-------------------------
        float timer = 0.0f;

        //------------------------
        //エフェクトを再生したか
        //------------------------
        bool effectPlayed = false;

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;

            //-------------------------
            // 指定時間でエフェクト再生
            //-------------------------
            if (!effectPlayed &&
                timer >= effectPlayTime)
            {
                effectPlayed = true;

                //-------------------------
                // エフェクト生成位置
                //-------------------------
                Vector3 pos =
                    transform.position -
                    transform.forward +
                    Vector3.up;

                //-------------------------
                // エフェクト生成
                //-------------------------
                EffekseerEmitter effect =
                    Instantiate(
                        PurificationPrefab,
                        pos,
                        transform.rotation);

                //-------------------------
                // エフェクト再生
                //-------------------------
                effect.Play();
            }

            // 0～1に正規化
            float alpha = Mathf.Clamp01(timer / fadeTime);

            //-------------------------
            // 紫を徐々に透明にする
            //-------------------------
            SetAlpha(purpleRenderer, 1.0f - alpha);

            //-------------------------
            // 青を徐々に表示する
            //-------------------------
            SetAlpha(blueRenderer, alpha);

            yield return null;
        }

        //-------------------------
        // フェード終了
        //-------------------------
        SetAlpha(purpleRenderer, 0.0f);
        SetAlpha(blueRenderer, 1.0f);

        //-------------------------
        // 紫ポータルを非表示
        //-------------------------
        if (purplePortalObj != null)
        {
            purplePortalObj.SetActive(false);
        }

        //-------------------------
        // 一定時間後に元へ戻す
        //-------------------------
        Invoke(nameof(ResetPortal), respawnTime);
    }

    //=========================
    // Renderer配列のアルファ値を変更
    //=========================
    void SetAlpha(Renderer[] renderers, float alpha)
    {
        if (renderers == null)
        {
            return;
        }

        foreach (Renderer r in renderers)
        {
            if (r == null)
            {
                continue;
            }

            foreach (Material mat in r.materials)
            {
                Color color = mat.color;
                color.a = alpha;
                mat.color = color;
            }
        }
    }

    //=========================
    // ポータルを元に戻して敵を再召喚する処理
    //=========================
    void ResetPortal()
    {
        //-------------------------
        // 浄化状態解除
        //-------------------------
        isPurified = false;

        //-------------------------
        // 紫ポータルを表示
        //-------------------------
        purplePortalObj.SetActive(true);

        //-------------------------
        // 青ポータルを表示
        //-------------------------
        bluePortalObj.SetActive(true);

        //-------------------------
        // アルファ値初期化
        //-------------------------
        SetAlpha(purpleRenderer, 1.0f);
        SetAlpha(blueRenderer, 0.0f);

        //-------------------------
        // 最後に青を非表示
        //-------------------------
        bluePortalObj.SetActive(false);

        //-------------------------
        // 敵を再召喚
        //-------------------------
        SpawnWave();
    }
}