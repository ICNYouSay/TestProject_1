using UnityEngine;

// ボスの状態遷移を管理するクラス
public class BossTransition : IEnemyTransition
{
    public EnemyState CheckTransition(EnemyBase enemy, EnemyState currentState)
    {
        BossEnemy boss = enemy as BossEnemy;
        if (boss == null) return null;

        // 死亡状態ならどんな状態からでも一瞬でBossDyingStateへ移行する
        if (boss.IsDead())
        {
            return boss.BossDyingState;
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

                        if (randomValue < 0.2f)
                        {
                            return boss.BiteState;   // 約20% かみつき
                        }
                        //else if (randomValue < 0.4f)
                        //{
                        //    return boss.ClawState;   // 約20% クロー
                        //}
                        //else if (randomValue < 0.6f)
                        //{
                        //    return boss.TailState;   // 約20% しっぽ
                        //}
                        //else if (randomValue < 0.8f)
                        //{
                        //    return boss.BreathState; // 約20% ブレス攻撃
                        //}
                        //else
                        //{
                        //    return boss.RushState;   // 20% 突進攻撃
                        //}

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
        // ブレス攻撃が終わったとき
        if (currentState is BreathState)
        {
            if (boss.IsAttackFinished())
            {
                return boss.IdleState;
            }
        }
        // 突進攻撃が終わったとき
        if (currentState is RushState)
        {
            if (boss.IsAttackFinished())
            {
                return boss.IdleState;
            }
        }
        return null;
    }
}