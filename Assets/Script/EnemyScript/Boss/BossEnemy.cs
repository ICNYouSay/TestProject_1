using UnityEngine;
using UnityEngine.Playables;

// ボス敵のベースクラス
public class BossEnemy : EnemyBase
{
    [Header("ボスの設定")]
    [SerializeField] private float moveSpeed;         // ボスの移動速度
    [SerializeField] private float attackRange;       // 攻撃が届く距離
    [SerializeField] private EnemyHitbox biteHitbox;  // かみつき攻撃の当たり判定
    [SerializeField] private EnemyHitbox clawHitbox;  // クロー攻撃の当たり判定
    [SerializeField] private EnemyHitbox tailHitbox;  // しっぽ攻撃の当たり判定
    [SerializeField] private EnemyHitbox breathHitbox;// ブレスの当たり判定
    [SerializeField] private EnemyHitbox rushHitbox;  // 突進の当たり判定

    private Animator anim;                            // アニメーション
    private CharacterController charController;       // CharacterController用
    private BiteState biteState;                      // かみつき攻撃のステート
    private ClawState clawState;                      // クロー攻撃のステート
    private TailState tailstate;                      // しっぽ攻撃のステート
    private BreathState breathState;                  // ブレス攻撃のステート
    private RushState rushState;                      // 突進攻撃のステート
    private BossDyingState bossDyingState;            // ボス死亡時のステート

    private float attackTimer;                        // 攻撃の硬直用タイマー

    public BiteState BiteState => biteState;
    public ClawState ClawState => clawState;
    public TailState TailState => tailstate;
    public BreathState BreathState => breathState;
    public RushState RushState => rushState;
    public BossDyingState BossDyingState => bossDyingState;
    public float AttackRange => attackRange;

    [Header("ボス撃破用のTimeLine")]
    [SerializeField]
    public PlayableDirector BossDefeatedTimeLine;     // ボス撃破時のTimeLineをInspectorで割り当てる

    // 二重実行防止
    private bool isStarting = false;


    protected override void Awake()
    {
        base.Awake();

        // ボス専用のTransitionをここに割り当てる
        transition = new BossTransition();

        // プレハブ内のGomegajiusからAnimatorを探す
        anim = GetComponentInChildren<Animator>();

        // CharacterControllerを取得する
        charController = GetComponent<CharacterController>();

        // ステートの生成
        biteState = new BiteState();
        clawState = new ClawState();
        tailstate = new TailState();
        breathState = new BreathState();
        rushState = new RushState();
        bossDyingState = new BossDyingState();
    }

    protected override void Update()
    {
        base.Update();

        anim.Update(Time.deltaTime);

        // デバッグ用 ボス撃破演出(カメラワーク)
        //if (Input.GetKeyDown(KeyCode.P) && !isStarting)
        //{
        //    // 演出を開始する
        //    isStarting = true;

        //    // ボスの状態を死亡ステートに切り替える
        //    ChangeState(bossDyingState);

        //    // ボス撃破カメラワーク開始
        //    BossDefeatedTimeLine.Play();
        //}

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

        float dist = Vector3.Distance(transform.position, player.transform.position);

        // 攻撃範囲より遠いときだけ移動する
        if (dist > attackRange)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }

            // プレイヤーに向かって移動する
            Vector3 moveDir = direction * moveSpeed;

            // 地面にしっかり接地させるための処理
            if (charController.isGrounded)
            {
                // 地面にいるときは下向きに押し付ける力を入れる
                moveDir.y = -2.0f;
            }
            else
            {
                // 空中にいるときは重力を加算していく
                moveDir.y += Physics.gravity.y * Time.deltaTime;
            }

            if (charController != null)
            {
                charController.Move(moveDir * Time.deltaTime);
            }

            // 動いているのでアニメーションを再生する
            PlayAnimation("Move");
        }
        else
        {
            // 攻撃範囲内に入ったら移動を止める
            StopMove();
        }
    }

    public override void StopMove()
    {
        base.StopMove();

        if (anim != null)
        {
            //anim.speed = 0.0f;
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
    // しっぽ攻撃の処理
    public void TailAttack()
    {
        Debug.Log("ボスの「しっぽ回転」処理実行！");
        if (tailHitbox != null)
        {
            tailHitbox.TriggerAttack();
        }
    }
    // ブレス攻撃の処理
    public void BreathAttack()
    {
        Debug.Log("ボスの「ブレス」処理実行！");
        if (breathHitbox != null)
        {
            breathHitbox.TriggerAttack();
        }
    }
    // 突進攻撃の処理
    public void RushAttack()
    {
        Debug.Log("ボスの「突進」処理実行！");
        if (rushHitbox != null)
        {
            rushHitbox.TriggerAttack();
        }
    }

    // 突進中の移動用関数
    public void MoveForRush(float speed)
    {
        Vector3 moveDir = transform.forward * speed;
        moveDir.y += Physics.gravity.y * Time.deltaTime;

        if (charController != null)
        {
            charController.Move(moveDir * Time.deltaTime);
        }
    }

    // 攻撃の終了処理
    public override bool IsAttackFinished()
    {
        attackTimer += Time.deltaTime;

        // 4秒間は攻撃中として待機に戻さない
        if (attackTimer >= 4.0f)
        {
            return true;
        }
        return false;
    }

    // アニメーションのトリガーを安全に引く共通関数
    public void PlayAnimation(string triggerName)
    {
        if (anim != null)
        {
            anim.SetTrigger(triggerName);
        }
    }

    // 死亡時のカメラ演出
    public void PlayDeathCamera()
    {
        Debug.Log("【デバッグ】PlayDeathCameraが呼ばれました！");

        if (BossDefeatedTimeLine != null)
        {
            Debug.Log("【デバッグ】Timelineが割り当てられています！再生します！");
            BossDefeatedTimeLine.Play();
        }
        else
        {
            Debug.LogError("【エラー】Timelineが割り当てられていません！");
        }

        // 演出が終わったあとにボスを消す(12秒後)
        StartCoroutine(DestroyBossAfterDelay(12.0f));
    }

    // ボスの消滅処理
    private System.Collections.IEnumerator DestroyBossAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // シーンを遷移する
        Debug.Log("ボス撃破！リザルトシーンへ移動します。");
        UnityEngine.SceneManagement.SceneManager.LoadScene("ResultScene");

        // その後ボスを消去
        var netObj = GetComponentInParent<Fusion.NetworkObject>();
        if (netObj != null && netObj.Runner != null)
        {
            netObj.Runner.Despawn(netObj);
            netObj.Runner.Shutdown();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

}