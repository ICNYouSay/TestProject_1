using Fusion;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

//プレイヤーのステータスを管理するクラス
public class PlayerStatus : MonoBehaviour
{
    [Header("キャラクターデータ")]
    [SerializeField]
    private CharacterData characterData;

    [Header("バフマネージャー")]
    [SerializeField]
    private BuffManager buffManager;

    //=================================
    //ネットワーク同期するステータス
    //=================================
    //最大HP
    [Networked]
    public float maxHP { get; private set; }

    //現在HP
    [Networked]
    public float currentHP { get; private set; }

    //攻撃力
    [Networked]
    public float baseAttack { get; private set; }
    public float Attack
    {
        get
        {
            return baseAttack + buffManager.GetAttackBuff();
        }
    }

    // 防御力
    [Networked]
    public float baseDefence { get; private set; }
    public float Defence
    {
        get
        {
            return baseDefence + buffManager.GetDefenceBuff();
        }
    }


    //========================
    // キャラクターデータから初期化
    //========================
    public void Initialize(CharacterData data)
    {
        //キャラクターデータを取得
        characterData = data;

        //最大体力をデータから取得(現在HPも設定)
        maxHP = data.hp;
        currentHP = maxHP;

        //攻撃力/防御力をデータから取得
        baseAttack = data.atk;
        baseDefence = data.def;

        Debug.Log(data.name + "の最大体力は" + maxHP);
        Debug.Log(data.name + "の現在体力は" + currentHP);
        Debug.Log(data.name + "の攻撃力は" + baseAttack);
        Debug.Log(data.name + "の防御力は" + baseDefence);

    }

    //--------------------
    // HPを減らす処理
    //--------------------
    public void TakeDamage(float damage)
    {
        // 最低1ダメージ保証
        float finalDamage = Mathf.Max(1, damage - Defence);

        currentHP -= finalDamage;

        Debug.Log("現在の体力 = " + currentHP);

        if (currentHP <= 0)
        {
            currentHP = 0;
        }
    }

    //========================
    // 回復処理
    //========================
    public void Heal(float healAmount)
    {
        currentHP += healAmount;

        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

    //========================
    // 死亡判定
    //========================
    public bool IsDead()
    {
        return currentHP <= 0;
    }

    //========================
    // CharacterData取得
    //========================
    public CharacterData CharacterData => characterData;

}
