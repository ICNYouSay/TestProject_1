using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    [Header("攻撃設定")]
    // プレイヤーに与えるダメージ
    [SerializeField] private float attackDamage;

    [Header("攻撃タイミング設定（秒）")]
    [Tooltip("攻撃開始から判定が出るまでの待ち時間（前隙）")]
    [SerializeField] private float startDelay;

    [Tooltip("判定が出ている時間")]
    [SerializeField] private float activeDuration;

    private BoxCollider _col;
    private SphereCollider _sphereCol;

    void Awake()
    {
        _col = GetComponent<BoxCollider>();
        _sphereCol = GetComponent<SphereCollider>();

        // 最初はOFF
        if (_col != null) _col.enabled = false;
        if (_sphereCol != null) _sphereCol.enabled = false; 
    }

    // 敵が攻撃する時に呼ぶ関数
    public void TriggerAttack()
    {
        // どちらもなければ終了
        if (_col == null && _sphereCol == null) return; 

        CancelInvoke();
        if (_col != null) _col.enabled = false;
        if (_sphereCol != null) _sphereCol.enabled = false; 

        Invoke(nameof(ActivateHitbox), startDelay);
        Invoke(nameof(DisableHitbox), startDelay + activeDuration);
    }

    private void ActivateHitbox()
    {
        // 判定ON
        if (_col != null) _col.enabled = true;
        if (_sphereCol != null) _sphereCol.enabled = true; 
    }

    public void DisableHitbox()
    {
        // 判定OFF
        if (_col != null) _col.enabled = false;
        if (_sphereCol != null) _sphereCol.enabled = false; 
    }

    private void OnDrawGizmos()
    {
        //// まずBoxColliderがあるか探す
        //BoxCollider boxCol = GetComponent<BoxCollider>();
        //if (boxCol != null)
        //{
        //    bool isActive = boxCol.enabled;

        //    // OFFなら描画しない
        //    if (!isActive) return; 

        //    Gizmos.color = Color.red;
        //    Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        //    // 描画
        //    Gizmos.DrawWireCube(boxCol.center, boxCol.size);
        //    return;
        //}

        //// SphereColliderがあるか探す
        //SphereCollider sphereCol = GetComponent<SphereCollider>();
        //if (sphereCol != null)
        //{
        //    bool isActive = sphereCol.enabled;

        //    // OFFなら描画しない
        //    if (!isActive) return; 

        //    Gizmos.color = Color.red;
        //    Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        //    // 描画
        //    Gizmos.DrawWireSphere(sphereCol.center, sphereCol.radius);
        //    return;
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        // 敵自身や敵同士への誤爆を防止
        if (other.gameObject == this.transform.root.gameObject || other.transform.IsChildOf(this.transform.root))
        {
            return;
        }

        // プレイヤーに当たった場合
        if (other.CompareTag("Player") || other.CompareTag("PlayerModel"))
        {
            // プレイヤーのステータススクリプトを取得
            PlayerStatus player = other.GetComponentInParent<PlayerStatus>();
            if (player != null)
            {
                // TakeDamegeを呼ぶ
                player.TakeDamage(attackDamage);
                Debug.Log($"【被弾】プレイヤーに {attackDamage} ダメージ！");
            }
        }
    }
}