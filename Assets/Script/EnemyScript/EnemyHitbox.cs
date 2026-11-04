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

    void Awake()
    {
        _col = GetComponent<BoxCollider>();

        // 最初はOFF
        if (_col != null) _col.enabled = false; 
    }

    // 敵が攻撃する時に呼ぶ関数
    public void TriggerAttack()
    {
        if (_col == null) return;

        CancelInvoke();
        _col.enabled = false;

        Invoke(nameof(ActivateHitbox), startDelay);
        Invoke(nameof(DisableHitbox), startDelay + activeDuration);
    }

    private void ActivateHitbox()
    {
        // 判定ON
        if (_col != null) _col.enabled = true; 
    }

    public void DisableHitbox()
    {
        // 判定OFF
        if (_col != null) _col.enabled = false; 
    }

    private void OnDrawGizmos()
    {
        BoxCollider col = GetComponent<BoxCollider>();
        if (col == null) return;

        bool isActive = col.enabled;
        // 敵はオレンジ〜赤
        Gizmos.color = isActive ? Color.red : new Color(1, 0.5f, 0, 0.2f);

        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.DrawWireCube(col.center, col.size);
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
                // player.TakeDamage(attackDamage);
                Debug.Log($"【被弾】プレイヤーに {attackDamage} ダメージ！");
            }
        }
    }
}