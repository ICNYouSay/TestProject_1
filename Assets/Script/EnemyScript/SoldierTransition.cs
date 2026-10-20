using UnityEngine;

//G‹›“G‚Ìó‘Ô‘JˆÚ‚ğŠÇ—‚·‚éƒNƒ‰ƒX
public class SoldierTransition : IEnemyTransition
{
    public EnemyState CheckTransition(
        EnemyBase enemy,
        EnemyState currentState)
    {
        SoldierEnemy soldier = enemy as SoldierEnemy;

        if (soldier == null)
            return null;

        //--------------------------------
        // Idle
        //--------------------------------
        if (currentState is IdleState)
        {
            //ƒvƒŒƒCƒ„[‚ğŒ©‚Â‚¯‚½‚çAttackState‚É‘JˆÚ‚·‚é
            if (soldier.CanSeePlayer())
            {
                return soldier.AttackState;
            }
        }

        //--------------------------------
        // Attack
        //--------------------------------
        if (currentState is AttackState)
        {
            //AttackState‚ªI—¹‚µ‚½‚çIdleState‚É‘JˆÚ‚·‚é
            if (soldier.IsAttackFinished())
            {
                return soldier.IdleState;
            }
        }

        return null;
    }
}