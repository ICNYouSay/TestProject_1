using UnityEngine;

public class AttackState : EnemyState
{
    public override void Enter(EnemyBase enemy)
    {
        // 攻撃アニメーション
        enemy.PlayAttackAnimation();

        // 攻撃開始
        enemy.Attack();
    }

    public override void Update(EnemyBase enemy)
    {
        // 攻撃中の処理
        //enemy.ChangeState(enemy.AttackState);

        enemy.Chase();
    }

    public override void Exit(EnemyBase enemy)
    {

    }
}
