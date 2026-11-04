using UnityEngine;

//雑魚敵
public class SoldierEnemy : EnemyBase
{
    //プレイヤーを見つけたかどうか
    [Header("プレイヤーを見つけたかどうか")]
    [SerializeField]
    private bool canSeePlayer= true;

    //攻撃が終了したかどうか
    [Header("攻撃が終了したかどうか")]
    [SerializeField]
    private bool attackFinished = true;

    [Header("攻撃設定")]
    [SerializeField]
    // プレイヤーにこの距離まで近づかないと攻撃しない
    private float attackRange; 

    [SerializeField]
    // 連携するHitboxスクリプト
    private EnemyHitbox enemyHitbox; 

    
    // 初期化
    protected override void Awake()
    {
        base.Awake();

        // 雑魚敵専用Transition
        transition = new SoldierTransition();
    }

    
    //更新
    protected override void Update()
    {
        base.Update();

        //テスト
        //もしBキーを押したら
        //バフ
        if (Input.GetKeyDown(KeyCode.B))
        {
            Buff buff = new Buff();

            buff.attack = 30;
            buff.duration = 30;

            //バフの追加
            enemyStatus.buffManager.AddBuff(buff);
        }


    }

    
    // プレイヤーを見つけたかどうかを返す
    public bool CanSeePlayer()
    {
        return canSeePlayer;
    }

    
    // 攻撃
    public override void Attack()
    {
        // まだ前の攻撃が終わっていないなら新しい攻撃を開始しない
        if (!attackFinished) return;

        // プレイヤーとの距離チェック
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerObj.transform.position);

        // 射程距離より遠ければ攻撃判定を出さずに処理を中断する
        if (distanceToPlayer > attackRange)
        {
            return;
        }

        Debug.Log("攻撃開始");

        attackFinished = false;

        // 実際の当たり判定を動かす
        if (enemyHitbox != null)
        {
            enemyHitbox.TriggerAttack();
        }
    }


}
