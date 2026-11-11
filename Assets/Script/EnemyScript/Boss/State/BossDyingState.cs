using UnityEngine;

// ボスの死亡State
public class BossDyingState : EnemyState
{
    public override void Enter(EnemyBase enemy)
    {
        BossEnemy boss = enemy as BossEnemy;
        if (boss != null)
        {
            // 動きを完全にとめる
            boss.StopMove();

            Debug.Log("【ボス死亡】DyingStateに突入しました！");

            // TODO: 後カメラ演出コードをここに組み込む
            // boss.PlayDeathCamera(); 
        }
    }

    public override void Update(EnemyBase enemy)
    {
        // 死亡演出中の処理
    }

    public override void Exit(EnemyBase enemy)
    {
    }
}