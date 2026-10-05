using UnityEngine;
using System.Collections.Generic;

public class EnemyWaveSpawner : MonoBehaviour
{
    // 召喚する敵のプレハブ
    public GameObject enemyPrefab;

    // 一度に召喚する数
    public int waveCount = 5;

    // 出現場所
    public Transform spawnPoint;    

    private List<GameObject> activeEnemies = new List<GameObject>();

    void Update()
    {
        // リストの中身を整理（破壊された敵をリストから除外）
        activeEnemies.RemoveAll(enemy => enemy == null);

        // もし敵が1体もいなかったら、まとめて召喚！
        if (activeEnemies.Count == 0)
        {
            SpawnWave();
        }
    }

    void SpawnWave()
    {
        for (int i = 0; i < waveCount; i++)
        {
            // i番目の敵を円周上に配置する計算
            // 5体なら72度ずつずらす
            float angle = i * (360f / waveCount);

            // 6fは半径
            Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad)) * 6f; 

            GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position + offset, spawnPoint.rotation);

            // 敵を召喚
            activeEnemies.Add(newEnemy);
        }
    }
}