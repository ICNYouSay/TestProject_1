using UnityEngine;

// 敵のステータスを管理するクラス
public class EnemyStatus : MonoBehaviour
{
    [Header("最大HP")]
    [SerializeField]
    private int maxHP = 1000;

    [Header("現在HP")]
    [SerializeField]
    private int currentHP;

    [Header("防御力")]
    [SerializeField]
    private int defence = 10;

    [Header("攻撃力")]
    [SerializeField]
    private int attackPower = 20;

    [Header("譲渡パワー量")]
    [SerializeField]
    private int givePower = 5;

    [Header("経験値")]
    [SerializeField]
    private int experience = 50;


    public event System.Action<int> OnDamage;

    //--------------------
    //初期化処理
    //--------------------
    private void Awake()
    {
        // 最大HPを現在HPに設定
        currentHP = maxHP;
    }

    //--------------------
    // HPを減らす処理
    //--------------------
    public void TakeDamage(int damage)
    {
        currentHP -= (damage - defence);

        Debug.Log("現在の体力 = " + currentHP);

        if (currentHP <= 0)
        {
            currentHP = 0;
        }
    }

    //--------------------
    // HPを回復する処理
    //--------------------
    public void Heal(int healAmount)
    {
        currentHP += healAmount;

        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

    //--------------------
    //死亡判定
    //--------------------
    public bool IsDead()       
    {
        // 現在のHPが0以下の場合、死亡と判定
        return currentHP <= 0;
    }

    //--------------------
    //Getter
    //--------------------
    public int CurrentHP => currentHP;// 現在のHPを取得するプロパティ

    public int MaxHP => maxHP;// 最大HPを取得するプロパティ

    public int Defence => defence;// 防御力を取得するプロパティ

    public int AttackPower => attackPower;// 攻撃力を取得するプロパティ

    public int GivePower => givePower;// 譲渡パワー量を取得するプロパティ

    public int Experience => experience;// 経験値を取得するプロパティ


}
