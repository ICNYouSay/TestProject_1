using UnityEngine;

//全Stateの基底クラス
public abstract class EnemyState
{
    //State開始時
    public virtual void Enter(EnemyBase enemy) { }

    //毎フレーム
    public virtual void Update(EnemyBase enemy) { }

    //Fixed更新
    public virtual void FixedUpdate(EnemyBase enemy) { }

    //State終了時
    public virtual void Exit(EnemyBase enemy) { }

}
