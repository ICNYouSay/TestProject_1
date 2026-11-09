using UnityEngine;

// かみつき攻撃
public class BiteState : EnemyState
{
    public override void Enter(EnemyBase enemy)
    {
        BossEnemy boss = enemy as BossEnemy;
        if (boss != null)
        {
            boss.StopMove();

            // 攻撃タイマーリセット
            boss.ResetAttackTimer(); 

            // 実際の噛みつき攻撃の処理やログ
            Debug.Log("ボスの「かみつき」攻撃！");

            boss.BiteAttack(); 
        }
    }

    public override void Update(EnemyBase enemy)
    {
        // 攻撃中の処理（必要なら書く）
    }

    public override void Exit(EnemyBase enemy)
    {
    }
}