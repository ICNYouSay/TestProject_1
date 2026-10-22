using NanoSockets;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

//敵キャラクターの基本的な機能を提供する抽象クラス
public abstract class EnemyBase : MonoBehaviour
{
    //敵キャラクターの状態を管理するステートマシン
    protected EnemyStateMachine stateMachine;

    //敵キャラクターの状態遷移を管理するインターフェース
    protected IEnemyTransition transition;

    //敵のステータスを管理するクラス
    protected EnemyStatus enemyStatus;

    //----------------------------
    // Stateインスタンス
    //----------------------------
    protected IdleState idleState;
    protected AttackState attackState;
    protected DyingState dyingState;

    //----------------------------
    // プロパティ
    //----------------------------
    //状態遷移を管理するインターフェースを取得するプロパティ
    public IEnemyTransition Transition => transition;

    //待機状態を取得するプロパティ
    public IdleState IdleState => idleState;

    //攻撃状態を取得するプロパティ
    public AttackState AttackState => attackState;

    //死亡状態を取得するプロパティ
    public DyingState DyingState => dyingState;


    //----------------------------
    // 初期化
    //----------------------------

    protected virtual void Awake()
    {
        // StateMachine生成
        stateMachine = new EnemyStateMachine();

        //敵のステータスを管理するクラスを取得
        enemyStatus = GetComponent<EnemyStatus>();

        // State生成
        idleState = new IdleState();//待機状態
        attackState = new AttackState();//攻撃状態
        dyingState = new DyingState();//死亡状態
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

        TakeDamage(11);
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
    //体力・移動・攻撃・アニメーション
    //---------------------------


    //=====================================
    //体力
    //=====================================
    //体力の減少処理
    public virtual void TakeDamage(int damage)
    {
        //ダメージを受けたときの処理
        enemyStatus.TakeDamage(damage);

        if (enemyStatus.IsDead())
        {
            //死亡処理(仮置き)
            ChangeState(dyingState); // 状態を解除
        }

    }

    //体力が0かどうかを判定する関数
    public virtual bool IsDead()
    {
        return enemyStatus.IsDead();
    }

    //死亡処理
    public virtual void Die()
    {
        //死亡処理
        //Debug.Log($"{name} is dead.");

        //上昇させる
        transform.Translate(Vector3.up * 2f);

    }

    //====================================

    //=====================================
    //移動    
    //=====================================
    //移動停止
    public virtual void StopMove()
    {
        //移動停止の処理
        //Debug.Log("移動停止");

        transform.Translate(Vector3.zero);
    }

    //追跡
    public virtual void Chase()
    {
    }
    //=====================================

    //=====================================
    //攻撃  
    //=====================================
    //攻撃開始
    public virtual void Attack()
    {

    }

    //攻撃終了したか
    public virtual bool IsAttackFinished()
    {
        return true;
    }
    //====================================

    //=====================================
    //アニメーション    
    //=====================================
    //Idleアニメーション
    public virtual void PlayIdleAnimation()
    {

    }

    //Attackアニメーション
    public virtual void PlayAttackAnimation()
    {

    }
    //=====================================
}