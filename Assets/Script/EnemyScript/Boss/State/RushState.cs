using UnityEngine;

// 突進攻撃
public class RushState : EnemyState
{
    private float rushTimer;              // 突進時間
    private bool isRushing;               // 今まさに突進中かどうかのフラグ
    private float delayBeforeRush = 1.0f; // 構える時間（秒）

    public override void Enter(EnemyBase enemy)
    {
        BossEnemy boss = enemy as BossEnemy;
        if (boss != null)
        {
            boss.StopMove();
            boss.ResetAttackTimer();
            rushTimer = 0f;

            // まだ突進していない状態からスタート
            isRushing = false;

            Debug.Log("ボスの「突進」予備動作！");
            boss.PlayAnimation("Rush");
        }
    }

    public override void Update(EnemyBase enemy)
    {
        BossEnemy boss = enemy as BossEnemy;
        if (boss == null) return;

        rushTimer += Time.deltaTime;

        // 構えが終わったか判定
        if (!isRushing && rushTimer >= delayBeforeRush)
        {
            isRushing = true;  // 突進開始
            rushTimer = 0f;    // 突進時間を測るためにリセット

            boss.RushAttack();
            Debug.Log("ボスの「突進」開始！");
        }

        // 突進中
        if (isRushing && rushTimer < 0.5f)
        {
            // CharacterController経由で突進させる
            boss.MoveForRush(50.0f); 
        }
    }

    public override void Exit(EnemyBase enemy)
    {
    }
}