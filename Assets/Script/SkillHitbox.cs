using UnityEngine;

public class SkillHitbox : MonoBehaviour
{
    private BoxCollider _col;

    void Awake()
    {
        _col = GetComponent<BoxCollider>();
    }

    // 攻撃開始時に呼ばれる
    public void EnableHitbox()
    {
        // オブジェクト自体を有効にする
        gameObject.SetActive(true); 
    }

    // 攻撃終了時に呼ばれる
    public void DisableHitbox()
    {
        // オブジェクト自体を無効にする
        gameObject.SetActive(false); 
    }

    // Sceneビューに判定を表示する機能
    private void OnDrawGizmos()
    {
        // 判定用コライダーを取得
        BoxCollider col = GetComponent<BoxCollider>();
        if (col == null) return;

        // 判定が有効なら「赤色」、無効なら「薄い緑色」で表示
        Gizmos.color = gameObject.activeSelf ? Color.red : new Color(0, 1, 0, 0.3f);

        // ワールド座標に合わせた位置とサイズで立方体を描画
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.DrawWireCube(col.center, col.size);
    }

    private void OnTriggerEnter(Collider other)
    {
        // もし「自分自身」または「自分の子オブジェクト」なら、ここで処理を終了する
        if (other.gameObject == this.transform.root.gameObject || other.transform.IsChildOf(this.transform.root))
        {
            return;
        }

        Debug.Log("判定に触れた！相手は： " + other.gameObject.name);

        if (other.CompareTag("Enemy"))
        {
            Debug.Log("【成功】敵にヒット！");
        }
    }
}