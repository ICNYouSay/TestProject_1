using UnityEngine;

public class Phase2TransitionState : EnemyState
{
    private float transitionTimer = 0f;
    private float transitionDuration = 2.0f; // 立ち上がる演出にかかる時間（秒）

    public override void Enter(EnemyBase enemy)
    {
        BossEnemy boss = enemy as BossEnemy;
        if (boss != null)
        {
            boss.StopMove();
            transitionTimer = 0f;
            Debug.Log("【形態変化】第2形態へ移行！");

            // 立ち上がるアニメーションを再生
            boss.PlayAnimation("Stand");
        }
    }

    public override void Update(EnemyBase enemy)
    {
        BossEnemy boss = enemy as BossEnemy;
        if (boss == null) return;

        transitionTimer += Time.deltaTime;

        // 演出時間が終わったら通常のIdleに戻す
        if (transitionTimer >= transitionDuration)
        {
            boss.ChangeState(boss.IdleState);
        }
    }

    public override void Exit(EnemyBase enemy)
    {
    }
}