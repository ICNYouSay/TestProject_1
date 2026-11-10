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
            // プレイヤーを見つけているか？
            if (boss.CanSeePlayer())
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    float dist = Vector3.Distance(boss.transform.position, player.transform.position);

                    // 攻撃範囲よりも遠ければ追いかける
                    if (dist > boss.AttackRange)
                    {
                        // プレイヤーを追いかける
                        boss.Chase();

                        // ステートはIdleのまま移動を続ける
                        return null;  
                    }
                    else
                    {
                        // 攻撃範囲内に入ったら移動を止めてかみつき攻撃に移行
                        boss.StopMove();
                        return boss.BiteState;
                    }
                }
            }
            else
            {
                // プレイヤーが見つからないときは移動を止める
                boss.StopMove();
            }
        }

        // かみつき攻撃のとき
        if (currentState is BiteState)
        {
            // 攻撃が終わったら待機状態に戻る
            if (boss.IsAttackFinished())
            {
                return boss.IdleState;
            }
        }

        return null;
    }
}