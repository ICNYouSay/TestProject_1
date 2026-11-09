using UnityEngine;

// ボス敵のベースクラス
public class BossEnemy : EnemyBase
{
    [Header("ボスの設定")]
    [SerializeField] private float moveSpeed = 3.0f;   // ボスの移動速度
    [SerializeField] private float attackRange = 4.0f; // 攻撃が届く距離

    protected override void Awake()
    {
        base.Awake();

        // ボス専用のTransitionをここに割り当てる
        transition = new BossTransition();
    }

    protected override void Update()
    {
        base.Update();
        // ボス特有の常時処理があればここに書く
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
    }

    // 攻撃の処理
    public override void Attack()
    {
        // ここにボスの攻撃処理を入れていく
        Debug.Log("ボスの攻撃！");
    }
}