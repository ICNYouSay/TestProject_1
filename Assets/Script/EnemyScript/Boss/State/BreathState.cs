using UnityEngine;

// ブレス攻撃
public class BreathState : EnemyState
{
    public override void Enter(EnemyBase enemy)
    {
        BossEnemy boss = enemy as BossEnemy;
        if (boss != null)
        {
            boss.StopMove();
            boss.ResetAttackTimer();

            // アニメーション
            boss.PlayAnimation("Breath");

            Debug.Log("ボスの「ブレス」攻撃！");
            boss.BreathAttack();
        }
    }

    public override void Update(EnemyBase enemy)
    {
    }

    public override void Exit(EnemyBase enemy)
    {
    }
}