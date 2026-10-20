using UnityEngine;

//敵キャラクターの状態遷移を管理するインターフェース
public interface IEnemyTransition
{
    EnemyState CheckTransition(
        EnemyBase enemy,
        EnemyState currentState);
}
