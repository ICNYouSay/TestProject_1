using NanoSockets;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

//敵キャラクターの基本的な機能を提供する抽象クラス
public abstract class EnemyBase : MonoBehaviour
{
    //敵キャラクターの状態を管理するステートマシン
    protected EnemyStateMachine stateMachine;

    //敵キャラクターの状態遷移を管理するインターフェース
    protected IEnemyTransition transition;

    //敵のステータスを管理するクラス
    protected EnemyStatus enemyStatus;

    // Stateインスタンス
    protected IdleState idleState;
    protected AttackState attackState;
    protected DyingState dyingState;

    // プロパティ
    //状態遷移を管理するインターフェースを取得するプロパティ
    public IEnemyTransition Transition => transition;

    //待機状態を取得するプロパティ
    public IdleState IdleState => idleState;

    //攻撃状態を取得するプロパティ
    public AttackState AttackState => attackState;

    //死亡状態を取得するプロパティ
    public DyingState DyingState => dyingState;
    // HPバーのGameObjectを保持する変数
    public GameObject HPBar; 
    // HPバーのImageコンポーネントを持つ2Dオブジェクトを保持する変数 
    public Image HPBar_Fill;

    // 初期化
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

    // 開始
    protected virtual void Start()
    {
        // 初期State
        ChangeState(idleState);

        // HPバーを非表示にする
        if (HPBar != null)
        {
            HPBar.SetActive(false); // HPバーを非表示にする
        }
    }

    // 更新
    protected virtual void Update()
    {
        stateMachine.Update(this);

        // HPBarがある時だけカメラを向くようにガード
        if (HPBar != null)
        {
            var mainCam = GameObject.FindGameObjectWithTag("MainCamera");
            if (mainCam != null)
            {
                HPBar.transform.LookAt(mainCam.transform);
            }
        }

        //テスト
        //TakeDamage(11);
    }

    // FixedUpdate

    protected virtual void FixedUpdate()
    {
        stateMachine.FixedUpdate(this);
    }

    // State変更
    public void ChangeState(EnemyState nextState)
    {
        stateMachine.ChangeState(this, nextState);
    }



    //体力の減少処理
    public virtual void TakeDamage(int damage)
    {
        //ダメージを受けたときの処理
        enemyStatus.TakeDamage(damage);

        if (HPBar != null)
        {
            HPBar.SetActive(true); // HPバーを表示する
        }

        if (HPBar_Fill != null)
        {
            HPBar_Fill.fillAmount = (float)enemyStatus.CurrentHP / enemyStatus.MaxHP; // HPバーの割合を更新
        }

        // HPが0以下になったら
        if (enemyStatus.IsDead())
        {
            ChangeState(dyingState); // 死亡状態へ

            // 当たり判定を即座に消す
            Collider[] colliders = GetComponentsInChildren<Collider>();
            foreach (var col in colliders)
            {
                col.enabled = false;
            }

            // 見た目を即座に非表示にして完全に消す
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (var ren in renderers)
            {
                ren.enabled = false;
            }

            // 本体を消滅させる（Photon Fusion対応）
            // もしFusionのNetworkObjectがついている場合
            var netObj = GetComponentInParent<Fusion.NetworkObject>();
            if (netObj != null && netObj.Runner != null)
            {
                netObj.Runner.Despawn(netObj);
            }
            else
            {
                // 通常のUnityオブジェクトならこれで完全に消える
                Destroy(this.gameObject);
            }

            Debug.Log($"【完全消滅】{gameObject.name} を消去しました！");
        }

    }

    //体力が0かどうかを判定する関数
    public virtual bool IsDead()
    {
        if(enemyStatus == null)
        {
            Debug.LogWarning("EnemyStatusがアタッチされていません。");
            return false;
        }

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


    //移動    
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

    //攻撃  
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