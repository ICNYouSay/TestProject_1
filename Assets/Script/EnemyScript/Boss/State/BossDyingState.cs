using UnityEngine;

// ボスの死亡State
public class BossDyingState : EnemyState
{
    public override void Enter(EnemyBase enemy)
    {
        BossEnemy boss = enemy as BossEnemy;
        if (boss != null)
        {
            // 動きや攻撃を完全に止める
            boss.StopMove();
            
            Debug.Log("【ボス撃破】BossDyingStateに突入！");

            // ボス側のカメラ演出
            boss.PlayDeathCamera();
        }
    }

    public override void Update(EnemyBase enemy)
    {
    }

    public override void Exit(EnemyBase enemy)
    {
    }
}