using UnityEngine;
using UnityEngine.UI;

public class BossHPBarManager : MonoBehaviour
{
    public BossEnemy targetBoss; // ここにボスをアタッチ
    public Image hpBarFill;      // 画面上のHPバーの画像

    void Update()
    {
        if (targetBoss != null && hpBarFill != null)
        {
            // 最大HPが0だと割り算でエラーになるので防止
            if (targetBoss.EnemyStatus.MaxHP <= 0) return;

            // ratioを0.0 ～ 1.0の間に強制的に収める
            float ratio = (float)targetBoss.EnemyStatus.CurrentHP / targetBoss.EnemyStatus.MaxHP;
            hpBarFill.fillAmount = Mathf.Clamp01(ratio);
        }
    }
}