using UnityEngine;

// 待機State
public class IdleState : EnemyState
{
    // State開始時
    public override void Enter(EnemyBase enemy)
    {
        // 移動停止
        enemy.StopMove();

        // 待機アニメーション(仮)
        enemy.PlayAttackAnimation();
    }

    // 毎フレーム
    public override void Update(EnemyBase enemy)
    {
    }

    // State終了時
    public override void Exit(EnemyBase enemy)
    {

    }
}