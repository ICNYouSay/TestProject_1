using UnityEngine;

// ボス専用のステータス（置いたまま安全に動く版）
public class BossStatus : MonoBehaviour
{
    [Header("最大HP")]
    [SerializeField]
    private float maxHP = 5000;

    [Header("現在HP")]
    [SerializeField]
    private float currentHP;

    [Header("基礎防御力")]
    [SerializeField]
    private float baseDefence = 20;

    [Header("基礎攻撃力")]
    [SerializeField]
    private float baseAttack = 50;

    [Header("譲渡パワー量")]
    [SerializeField]
    private float givePower = 5;

    [Header("経験値")]
    [SerializeField]
    private float experience = 50;

    [Header("バフマネージャー")]
    [SerializeField]
    public BuffManager buffManager;

    public event System.Action<float> OnDamage;

    // バフ込みの攻撃力
    public float AttackPower
    {
        get
        {
            if (buffManager == null)
                return baseAttack;

            return baseAttack + buffManager.GetAttackBuff();
        }
    }

    // バフ込みの防御力
    public float Defence
    {
        get
        {
            if (buffManager == null)
                return baseDefence;

            return baseDefence + buffManager.GetDefenceBuff();
        }
    }

    // 初期化（ゲームが始まった瞬間にHPを満タンにする）
    private void Awake()
    {
        currentHP = maxHP;
        Debug.Log($"【ボス初期化】HPを満タン（{maxHP}）に設定しました！");
    }

    // ダメージ処理
    public void TakeDamage(float damage)
    {
        // バフ込み防御力を使用
        float finalDamage = Mathf.Max(0, damage - Defence);

        currentHP -= finalDamage;

        if (currentHP < 0)
        {
            currentHP = 0;
        }

        Debug.Log($"【ボス被弾】{finalDamage} ダメージを受けた！ 残りHP: {currentHP}");

        OnDamage?.Invoke(finalDamage);
    }

    // 死亡判定
    public bool IsDead()
    {
        return currentHP <= 0;
    }

    // 他のプログラムから値を見るための窓口
    public float CurrentHP => currentHP;
    public float MaxHP => maxHP;
    public float GivePower => givePower;
    public float Experience => experience;
}