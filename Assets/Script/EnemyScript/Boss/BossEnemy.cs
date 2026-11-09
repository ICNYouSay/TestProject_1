using UnityEngine;

// ボス敵のベースクラス
public class BossEnemy : EnemyBase
{
    [Header("ボスの設定")]
    [SerializeField] private float moveSpeed ;        // ボスの移動速度
    [SerializeField] private float attackRange;       // 攻撃が届く距離
    [SerializeField] private EnemyHitbox biteHitbox;  // かみつき攻撃の当たり判定
    [SerializeField] private EnemyHitbox clawHitbox;  // クロー攻撃の当たり判定
    [SerializeField] private float attackDuration;    // 1回の攻撃にかかる時間（秒）

    private float attackTimer = 0.0f;
    private Animator anim;                            // アニメーション
    private BiteState biteState;                      // かみつき攻撃のステート
    private ClawState clawState;                      // クロー攻撃のステート

    public BiteState BiteState => biteState;
    public ClawState ClawState => clawState;
    public float AttackRange => attackRange;

    protected override void Awake()
    {
        base.Awake();

        // ボス専用のTransitionをここに割り当てる
        transition = new BossTransition();

        // プレハブ内のGomegajiusからAnimatorを探す
        anim = GetComponentInChildren<Animator>();

        // ステートの生成
        biteState = new BiteState();
        clawState = new ClawState();
    }

    protected override void Update()
    {
        base.Update();
    }

    // プレイヤーを見つけているか
    public bool CanSeePlayer()
    {
        // 簡易的に常にtrueにするか、プレイヤーとの距離で判定
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return false;

        float dist = Vector3.Distance(transform.position, player.transform.position);

        // 例えば20メートル以内なら発見している扱いにするなど
        return dist < 20.0f;
    }

    // 追跡の処理をボス用に中身を埋める
    public override void Chase()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        // プレイヤーの方向を向く
        Vector3 direction = (player.transform.position - transform.position).normalized;
        // 上下には傾かせない
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        // プレイヤーに向かって移動する
        transform.position += direction * moveSpeed * Time.deltaTime;

        // 動いているのでアニメーションを再生する
        if (anim != null)
        {
            anim.speed = 1.0f;
        }
    }
    public override void StopMove()
    {
        base.StopMove();

        // 止まったのでアニメーションを止める
        if (anim != null)
        {
            anim.speed = 0.0f;
        }
    }
    public void BiteAttack()
    {
        // かみつき処理
        Debug.Log("ボスの「かみつき」処理実行！");

        // 当たり判定の処理
        if (biteHitbox != null)
        {
            biteHitbox.TriggerAttack();
        }

    }

    public void ClawAttack()
    {
        Debug.Log("ボスの「クロー」処理実行！");

        // 当たり判定の処理
        if (clawHitbox != null)
        {
            clawHitbox.TriggerAttack(); 
        }
    }

    // 攻撃の終了判定
    public override bool IsAttackFinished()
    {
        // 経過時間を進める
        attackTimer += Time.deltaTime;

        // 指定した秒数が経過するまでは攻撃中にする
        if (attackTimer >= attackDuration)
        {
            return true; // 攻撃終了
        }

        return false; 
    }

    // 攻撃のステートに入ったときにタイマーをリセットする用
    public void ResetAttackTimer()
    {
        attackTimer = 0f;
    }

}