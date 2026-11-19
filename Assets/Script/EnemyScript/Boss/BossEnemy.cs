using UnityEngine;
using UnityEngine.Playables;

// ボス敵のベースクラス
public class BossEnemy : EnemyBase
{
    [Header("ボスの設定")]
    [SerializeField] private float moveSpeed;                     // ボスの移動速度
    [SerializeField] private float attackRange;                   // 攻撃が届く距離
    [SerializeField] private EnemyHitbox biteHitbox;              // かみつき攻撃の当たり判定
    [SerializeField] private EnemyHitbox clawHitbox;              // クロー攻撃の当たり判定
    [SerializeField] private EnemyHitbox tailHitbox;              // しっぽ攻撃の当たり判定
    [SerializeField] private GameObject fireballPrefab;           // ブレスの弾のプレハブ
    [SerializeField] private Transform  fireballPoint;            // 口元の発射位置
    [SerializeField] private float breathDelay;                   // 溜めモーションの待ち時間（秒）
    [SerializeField] private EnemyHitbox rushHitbox;              // 突進の当たり判定

    [Header("形態変化の設定")]
    [SerializeField] private bool isPhase2 = false;               // 第2形態の判定
    
    private Animator anim;                                        // アニメーション
    private CharacterController charController;                   // CharacterController用
    private BiteState biteState;                                  // かみつき攻撃のステート
    private ClawState clawState;                                  // クロー攻撃のステート
    private TailState tailstate;                                  // しっぽ攻撃のステート
    private BreathState breathState;                              // ブレス攻撃のステート
    private RushState rushState;                                  // 突進攻撃のステート
    private Phase2TransitionState phase2TransitionState;          // 第2形態のステート
    private BossDyingState bossDyingState;                        // ボス死亡時のステート

    private float attackTimer;                                    // 攻撃の硬直用タイマー

    [Header("ボスの状態フラグ")]
    public bool isDefeated = false; 　　　　　　　　　　　　　　　// ボス撃破フラグ

    public BiteState BiteState => biteState;
    public ClawState ClawState => clawState;
    public TailState TailState => tailstate;
    public BreathState BreathState => breathState;
    public RushState RushState => rushState;
    public Phase2TransitionState Phase2TransitionState => phase2TransitionState;
    public BossDyingState BossDyingState => bossDyingState;
    public float AttackRange => attackRange;
    public bool IsPhase2 => isPhase2;

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
        phase2TransitionState = new Phase2TransitionState();
        bossDyingState = new BossDyingState();
    }

    protected override void Update()
    {
        base.Update();

        anim.Update(Time.deltaTime);

    }

    // プレイヤーを見つけているか
    public bool CanSeePlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return false;

        // ボスの索敵範囲
        float dist = Vector3.Distance(transform.position, player.transform.position);

        return dist < 60.0f;
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

            // 歩きアニメーション
            if (isPhase2)
            {
                PlayAnimation("StandMove"); // 第2形態の歩きアニメーション
            }
            else
            {
                PlayAnimation("Move");      // 通常の歩きアニメーション
            }

        }
        else
        {
            // 攻撃範囲内に入ったら移動を止める
            StopMove();
        }
    }

    // プレイヤーからダメージを受けたときの処理
    public override void TakeDamage(int damage)
    {
        // 通常のダメージ処理を呼ぶ
        base.TakeDamage(damage);

        // まだ第2形態ではなくHPが最大HPの半分以下になった時
        if (!isPhase2 && enemyStatus != null)
        {
            if (enemyStatus.CurrentHP <= enemyStatus.MaxHP / 2)
            {
                TriggerPhase2();
                ChangeState(phase2TransitionState);
            }
        }
    }

    // 第2形態に移行するときの処理
    private void TriggerPhase2()
    {
        isPhase2 = true;
        Debug.Log("ボスのHPが半分を切った！第2形態に突入！");

        // ここで第2形態の強化を行う
        moveSpeed += 3.0f; // 移動速度アップ

    }

    public override void StopMove()
    {
        base.StopMove();

        // アニメーションを止める
        //PlayAnimation("Stop");

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
        Debug.Log("ボスの「ブレス」予備動作スタート！");

        // 溜め時間を待ってから弾を発射を開始
        StartCoroutine(ShootFireballAfterDelay(breathDelay));
    }

    // 溜めが終わった瞬間に弾を生み出す
    private System.Collections.IEnumerator ShootFireballAfterDelay(float delay)
    {
        // 指定した溜め時間待つ
        yield return new WaitForSeconds(delay);

        // 溜めが終わった瞬間に弾を生成
        if (fireballPrefab != null && fireballPoint != null)
        {
            Instantiate(fireballPrefab, fireballPoint.position, fireballPoint.rotation);
            Debug.Log("火球を発射しました！");
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
            // もし今すでにそのアニメーションを再生中なら連打して巻き戻さない
            if (anim.GetCurrentAnimatorStateInfo(0).IsName(triggerName))
            {
                return; // すでに再生中だから何もしない
            }

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

        // 撃破フラグをONにする
        isDefeated = true;
        Debug.Log("【ボス完全撃破】撃破フラグをONにしました！");

        // GameLauncherを探してボスが倒されたことを直接通知する
        GameLauncher launcher = FindFirstObjectByType<GameLauncher>();
        if (launcher != null)
        {
            launcher.OnBossDefeated();
        }
        else
        {
            // 万が一GameLauncherが見つからない場合の保険
            UnityEngine.SceneManagement.SceneManager.LoadScene("ResultScene");
        }

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