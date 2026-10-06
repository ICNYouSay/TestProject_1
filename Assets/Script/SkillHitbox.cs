using UnityEngine;

public class SkillHitbox : MonoBehaviour
{
    private Collider _col;

    void Awake()
    {
        _col = GetComponent<Collider>();

        // 最初はオフ
        _col.enabled = false; 
    }

    // 攻撃開始（PlayerControllerから呼ぶ）
    public void EnableHitbox() => _col.enabled = true;

    // 攻撃終了（PlayerControllerから呼ぶ）
    public void DisableHitbox() => _col.enabled = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("グングニルがヒット！");
            // ここで敵にダメージを与える処理を呼ぶ
        }
    }
}