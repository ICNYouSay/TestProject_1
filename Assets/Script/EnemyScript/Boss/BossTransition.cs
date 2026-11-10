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

        // 待機状態のとき
        if (currentState is IdleState)
        {
            if (boss.CanSeePlayer())
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    float dist = Vector3.Distance(boss.transform.position, player.transform.position);

                    // 攻撃範囲よりも遠ければ追いかける
                    if (dist > boss.AttackRange)
                    {
                        boss.Chase();
                        return null;
                    }
                    else
                    {
                        // 攻撃範囲内に入ったら移動を止め、50%の確率でかみつきかクローに移行
                        boss.StopMove();

                        float randomValue = Random.value;
                        if (randomValue < 0.5f)
                        {
                            return boss.BiteState;
                        }
                        else
                        {
                            return boss.ClawState;
                        }
                    }
                }
            }
            else
            {
                boss.StopMove();
            }
        }

        // かみつき攻撃が終わったとき
        if (currentState is BiteState)
        {
            if (boss.IsAttackFinished())
            {
                return boss.IdleState;
            }
        }

        // クロー攻撃が終わったとき
        if (currentState is ClawState)
        {
            if (boss.IsAttackFinished())
            {
                return boss.IdleState;
            }
        }

        return null;
    }
}