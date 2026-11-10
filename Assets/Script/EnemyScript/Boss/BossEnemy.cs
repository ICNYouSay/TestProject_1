using UnityEngine;

// ボス敵のベースクラス
public class BossEnemy : EnemyBase
{
    [Header("ボスの設定")]
    [SerializeField] private float moveSpeed;         // ボスの移動速度
    [SerializeField] private float attackRange;       // 攻撃が届く距離
    [SerializeField] private EnemyHitbox biteHitbox;  // かみつき攻撃の当たり判定
    [SerializeField] private EnemyHitbox clawHitbox;  // クロー攻撃の当たり判定
    [SerializeField] private EnemyHitbox tailHitbox; // しっぽ用の円形ヒットボックス

    private Animator anim;                            // アニメーション
    private BiteState biteState;                      // かみつき攻撃のステート
    private ClawState clawState;                      // クロー攻撃のステート
    private TailState tailstate;                      // しっぽ回転攻撃のステート

    public BiteState BiteState => biteState;
    public ClawState ClawState => clawState;
    public TailState TailState => tailstate;
    public float AttackRange => attackRange;

    private float attackTimer;                   // 攻撃の硬直用タイマー
  
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
        tailstate = new TailState();
    }

    protected override void Update()
    {
        base.Update();
    }

    // プレイヤーを見つけているか
    public bool CanSeePlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return false;

        float dist = Vector3.Distance(transform.position, player.transform.position);
        return dist < 20.0f;
    }

    // 追跡の処理
    public override void Chase()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        transform.position += direction * moveSpeed * Time.deltaTime;

        if (anim != null)
        {
            anim.speed = 1.0f;
        }
    }

    public override void StopMove()
    {
        base.StopMove();

        if (anim != null)
        {
            anim.speed = 0.0f;
        }
    }

    // 攻撃タイマーのリセット用
    public void ResetAttackTimer()
    {
        attackTimer = 0f;
    }

    // かみつき攻撃の処理
    public void BiteAttack()
    {
        Debug.Log("ボスの「かみつき」処理実行！");
        if (biteHitbox != null)
        {
            biteHitbox.TriggerAttack();
        }
    }

    // クロー攻撃の処理
    public void ClawAttack()
    {
        Debug.Log("ボスの「クロー」処理実行！");
        if (clawHitbox != null)
        {
            clawHitbox.TriggerAttack();
        }
    }
    public void TailAttack()
    {
        Debug.Log("ボスの「しっぽ回転」処理実行！");
        if (tailHitbox != null)
        {
            tailHitbox.TriggerAttack();
        }
    }
    public override bool IsAttackFinished()
    {
        attackTimer += Time.deltaTime;

        // 2秒間は攻撃中として待機に戻さない
        if (attackTimer >= 2.0f)
        {
            return true;
        }
        return false;
    }
}