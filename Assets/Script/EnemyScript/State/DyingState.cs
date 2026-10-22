using UnityEngine;

public class DyingState : EnemyState
{
    public override void Enter(EnemyBase enemy)
    {
        //ˆÚ“®’â~
        enemy.StopMove();
    }

    public override void Update(EnemyBase enemy)
    {
        //€–S‚Ìˆ—
        enemy.Die();
    }

    public override void Exit(EnemyBase enemy)
    {

    }
}
