using UnityEngine;

//StateMachineクラス
public class EnemyStateMachine
{
    //現在のState
    public EnemyState currentState { get; private set; }

    //State変更
    public void ChangeState(EnemyBase enemy, EnemyState nextState)
    {
        // 同じStateなら変更しない
        if (currentState == nextState)
        {
            return;
        }

        // 現在のState終了
        currentState?.Exit(enemy);

        // State変更
        currentState = nextState;

        // 新しいState開始
        currentState?.Enter(enemy);
    }

    //更新
    public void Update(EnemyBase enemy)
    {
        // State更新
        currentState?.Update(enemy);

        // 遷移判定
        EnemyState next =
            enemy.Transition.CheckTransition(
                enemy,
                currentState);

        // State変更
        if (next != null)
        {
            ChangeState(enemy, next);
        }
    }

    //Fixed更新
    public void FixedUpdate(EnemyBase enemy)
    {
        currentState?.FixedUpdate(enemy); //現在のStateFixed更新処理
    }
}
