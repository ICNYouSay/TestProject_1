using UnityEngine;

// 敵のステータスを管理するクラス
public class EnemyStatus : MonoBehaviour
{
    [Header("最大HP")]
    [SerializeField]
    private float maxHP = 1000;

    [Header("現在HP")]
    [SerializeField]
    private float currentHP;

    [Header("基礎防御力")]
    [SerializeField]
    private float baseDefence = 10;

    [Header("基礎攻撃力")]
    [SerializeField]
    private float baseAttack = 20;

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

    //========================
    // バフ込みの攻撃力
    //========================
    public float AttackPower
    {
        get
        {
            if (buffManager == null)
                return baseAttack;

            return baseAttack + buffManager.GetAttackBuff();
        }
    }

    //========================
    // バフ込みの防御力
    //========================
    public float Defence
    {
        get
        {
            if (buffManager == null)
                return baseDefence;

            return baseDefence + buffManager.GetDefenceBuff();
        }
    }

    //--------------------
    // 初期化
    //--------------------
    private void Awake()
    {
        currentHP = maxHP;
    }

    //--------------------
    // ダメージ処理
    //--------------------
    public void TakeDamage(float damage)
    {
        // バフ込み防御力を使用
        float finalDamage = Mathf.Max(0, damage - Defence);

        currentHP -= finalDamage;

        if (currentHP < 0)
        {
            currentHP = 0;
        }

        OnDamage?.Invoke(finalDamage);
    }

    //--------------------
    // 回復
    //--------------------
    public void Heal(float healAmount)
    {
        currentHP += healAmount;

        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

    //--------------------
    // 死亡判定
    //--------------------
    public bool IsDead()
    {
        return currentHP <= 0;
    }

    //--------------------
    // Getter
    //--------------------
    public float CurrentHP => currentHP;

    public float MaxHP => maxHP;

    public float GivePower => givePower;

    public float Experience => experience;
}