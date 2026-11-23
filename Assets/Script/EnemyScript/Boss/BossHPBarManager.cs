using UnityEngine;
using UnityEngine.UI;

public class BossHPBarManager : MonoBehaviour
{
    public BossEnemy targetBoss; // ここにボス（BoosRoot）をセットする
    public Image hpBarFill;      // 画面上のHPバーの画像

    private BossStatus bossStatus;

    void Start()
    {
        // もしインスペクターでボスが入っていなければ、自動で探す
        if (targetBoss == null)
        {
            targetBoss = FindFirstObjectByType<BossEnemy>();
        }

        if (targetBoss != null)
        {
            bossStatus = targetBoss.GetComponent<BossStatus>();
        }
    }

    void Update()
    {
        if (bossStatus == null && targetBoss != null)
        {
            bossStatus = targetBoss.GetComponent<BossStatus>();
        }

        // ボスと画像とステータスがそろっていればバーを更新する
        if (targetBoss != null && hpBarFill != null && bossStatus != null)
        {
            if (bossStatus.MaxHP <= 0) return;

            float ratio = (float)bossStatus.CurrentHP / bossStatus.MaxHP;
            hpBarFill.fillAmount = Mathf.Clamp01(ratio);
        }
    }
}