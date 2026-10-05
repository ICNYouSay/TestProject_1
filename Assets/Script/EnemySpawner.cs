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
            // 少しずつ位置をずらして召喚（同じ場所に重なると敵が吹き飛ぶのを防ぐため）
            Vector3 randomOffset = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position + randomOffset, spawnPoint.rotation);

            // 生成した敵をリストに追加
            activeEnemies.Add(newEnemy);
        }
    }
}