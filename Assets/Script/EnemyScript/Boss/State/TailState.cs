using UnityEngine;

// しっぽ回転攻撃
public class TailState : EnemyState
{
    public override void Enter(EnemyBase enemy)
    {
        BossEnemy boss = enemy as BossEnemy;
        if (boss != null)
        {
            boss.StopMove();
            boss.ResetAttackTimer();

            // アニメーション
            boss.PlayAnimation("Tail");

            Debug.Log("ボスの「しっぽ回転」攻撃！");
            boss.TailAttack();
        }
    }

    public override void Update(EnemyBase enemy)
    {
    }

    public override void Exit(EnemyBase enemy)
    {
    }
}