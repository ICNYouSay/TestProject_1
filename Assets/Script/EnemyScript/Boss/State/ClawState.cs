using UnityEngine;

// クロー攻撃
public class ClawState : EnemyState
{
    public override void Enter(EnemyBase enemy)
    {
        BossEnemy boss = enemy as BossEnemy;
        if (boss != null)
        {
            boss.StopMove();

            // 攻撃タイマーリセット
            boss.ResetAttackTimer();

            // クロー攻撃の処理やログ
            Debug.Log("ボスの爪攻撃！");

            boss.ClawAttack();
        }
    }

    public override void Update(EnemyBase enemy)
    {
    }

    public override void Exit(EnemyBase enemy)
    {
    }
}