using UnityEngine;

// 突進攻撃
public class RushState : EnemyState
{
    private float rushTimer;

    public override void Enter(EnemyBase enemy)
    {
        BossEnemy boss = enemy as BossEnemy;
        if (boss != null)
        {
            // 突進なので一瞬足を止めるか、あるいは急発進させる
            boss.StopMove();
            boss.ResetAttackTimer();
            rushTimer = 0f;

            // アニメーション
            //boss.PlayAnimation("Rush");

            Debug.Log("ボスの「突進」攻撃！");

            boss.RushAttack();
        }
    }

    public override void Update(EnemyBase enemy)
    {
        BossEnemy boss = enemy as BossEnemy;
        if (boss != null)
        {
            // 突進中は前方に移動する
            rushTimer += Time.deltaTime;
            if (rushTimer < 0.5f)
            {
                // ボスの向いている方向に高速移動
                boss.transform.position += boss.transform.forward * 30.0f * Time.deltaTime;
            }
        }
    }

    public override void Exit(EnemyBase enemy)
    {
    }
}