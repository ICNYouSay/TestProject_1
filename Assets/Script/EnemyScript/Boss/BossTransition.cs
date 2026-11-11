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
                        // 攻撃範囲内に入ったら移動を止める
                        boss.StopMove();

                        // 確率で攻撃を変更
                        float randomValue = Random.value;

                        if (randomValue < 0.33f)
                        {
                            return boss.BiteState; // 約33% かみつき
                        }
                        else if (randomValue < 0.66f)
                        {
                            return boss.ClawState; // 約33% クロー
                        }
                        else
                        {
                            return boss.TailState; // 残り約34% しっぽ回転
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

        // しっぽ回転攻撃が終わったとき
        if (currentState is TailState)
        {
            if (boss.IsAttackFinished())
            {
                return boss.IdleState;
            }
        }

        return null;
    }
}