using UnityEngine;

// ボスの状態遷移を管理するクラス
public class BossTransition : IEnemyTransition
{
    public EnemyState CheckTransition(EnemyBase enemy, EnemyState currentState)
    {
        BossEnemy boss = enemy as BossEnemy;
        if (boss == null) return null;

        // 死亡状態なら何もしない
        if (boss.IsDead())
        {
            return boss.DyingState;
        }

        // Idle状態
        if (currentState is IdleState)
        {
            // プレイヤーを見つけたらAttackStateへ
            if (boss.CanSeePlayer())
            {
                return boss.AttackState;
            }
        }

        // Attack状態
        if (currentState is AttackState)
        {
            // 攻撃が終わったらIdleに戻る
            if (boss.IsAttackFinished())
            {
                return boss.IdleState;
            }
        }

        return null;
    }
}