using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

//敵キャラクターの基本的な機能を提供する抽象クラス
public abstract class EnemyBase : MonoBehaviour
{
    //敵キャラクターの状態を管理するステートマシン
    protected EnemyStateMachine stateMachine;

    //敵キャラクターの状態遷移を管理するインターフェース
    protected IEnemyTransition transition;

    //----------------------------
    // Stateインスタンス
    //----------------------------
    protected IdleState idleState;
    protected AttackState attackState;

    //----------------------------
    // プロパティ
    //----------------------------
    //状態遷移を管理するインターフェースを取得するプロパティ
    public IEnemyTransition Transition => transition;

    //待機状態を取得するプロパティ
    public IdleState IdleState => idleState;

    //攻撃状態を取得するプロパティ
    public AttackState AttackState => attackState;

    //----------------------------
    // 初期化
    //----------------------------

    protected virtual void Awake()
    {
        // StateMachine生成
        stateMachine = new EnemyStateMachine();

        // State生成
        idleState = new IdleState();//待機状態
        attackState = new AttackState();//攻撃状態
    }

    //----------------------------
    // 開始
    //----------------------------

    protected virtual void Start()
    {
        // 初期State
        ChangeState(idleState);
    }

    //----------------------------
    // 更新
    //----------------------------

    protected virtual void Update()
    {
        stateMachine.Update(this);
    }

    //----------------------------
    // FixedUpdate
    //----------------------------

    protected virtual void FixedUpdate()
    {
        stateMachine.FixedUpdate(this);
    }

    //----------------------------
    // State変更
    //----------------------------
    public void ChangeState(EnemyState nextState)
    {
        stateMachine.ChangeState(this, nextState);
    }

    //----------------------------
    // 共通関数
    //----------------------------
    //移動停止
    public virtual void StopMove()
    {

    }

    //追跡
    public virtual void Chase()
    {
    }

    //周囲を見渡す
    public virtual void LookAround()
    {
    }

    //攻撃開始
    public virtual void Attack()
    {

    }

    //攻撃終了したか
    public virtual bool IsAttackFinished()
    {
        return true;
    }

    //Idleアニメーション
    public virtual void PlayIdleAnimation()
    {

    }

    //Attackアニメーション
    public virtual void PlayAttackAnimation()
    {

    }
}