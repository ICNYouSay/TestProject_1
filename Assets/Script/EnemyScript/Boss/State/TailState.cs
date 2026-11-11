using UnityEngine;

// ‚µ‚Á‚Û‰ñ“]UŒ‚
public class TailState : EnemyState
{
    public override void Enter(EnemyBase enemy)
    {
        BossEnemy boss = enemy as BossEnemy;
        if (boss != null)
        {
            boss.StopMove();
            boss.ResetAttackTimer();
            Debug.Log("ƒ{ƒX‚Ìu‚µ‚Á‚Û‰ñ“]vUŒ‚I");
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