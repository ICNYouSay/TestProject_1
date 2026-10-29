using UnityEngine;

public class SkillHitbox : MonoBehaviour
{
    [Header("攻撃設定")]
    [SerializeField] private int attackDamage;

    [Header("攻撃タイミング設定（秒）")]
    [Tooltip("ボタンを押してから判定が出るまでの待ち時間（前隙）")]
    [SerializeField] private float startDelay;    // 攻撃を突き出す瞬間の秒数

    [Tooltip("判定が出ている持続時間")]
    [SerializeField] private float activeDuration; // 判定を残す秒数

    private BoxCollider _col;

    void Awake()
    {
        _col = GetComponent<BoxCollider>();
        if (_col != null) _col.enabled = false; // 最初はOFF
    }

    // スキル発動時に呼ぶ
    // 待ち時間 → ON → OFF を自動で行う
    public void TriggerHitbox()
    {
        if (_col == null) return;

        // 過去の予約をリセット＆確実にOFFにしておく
        CancelInvoke();
        _col.enabled = false;

        // 指定した秒数後に判定をONにする
        Invoke(nameof(ActivateHitbox), startDelay);

        // その後さらにactiveDuration秒後に判定をOFFにする
        Invoke(nameof(DisableHitbox), startDelay + activeDuration);
    }

    // 引数で「ダメージ」「待ち時間」「持続時間」をその都度指定できる便利バージョン
    public void TriggerHitbox(int damage, float delay, float duration)
    {
        attackDamage = damage;
        startDelay = delay;
        activeDuration = duration;

        // 通常の起動処理を呼ぶ
        TriggerHitbox();
    }

    private void ActivateHitbox()
    {
        if (_col != null) _col.enabled = true; // 判定ON
    }

    public void DisableHitbox()
    {
        if (_col != null) _col.enabled = false; // 判定OFF
    }

    private void OnDrawGizmos()
    {
        BoxCollider col = GetComponent<BoxCollider>();
        if (col == null) return;

        bool isActive = col.enabled;
        Gizmos.color = isActive ? Color.red : new Color(0, 1, 0, 0.2f);

        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.DrawWireCube(col.center, col.size);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == this.transform.root.gameObject || other.transform.IsChildOf(this.transform.root))
        {
            return;
        }

        if (other.CompareTag("Enemy"))
        {
            EnemyBase enemy = other.GetComponentInParent<EnemyBase>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
                Debug.Log($"【命中】{enemy.gameObject.name} に {attackDamage} ダメージ！");
            }
        }
    }
}