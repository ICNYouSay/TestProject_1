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
            boss.ResetAttackTimer();

            // アニメーション
            boss.PlayAnimation("Bite");

            Debug.Log("ボスの「かみつき」攻撃！");
            boss.BiteAttack();
        }
    }

    public override void Update(EnemyBase enemy)
    {
    }

    public override void Exit(EnemyBase enemy)
    {
    }
}