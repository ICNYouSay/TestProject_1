using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Buff
{
    // 攻撃力補正
    public float attack;

    // 防御力補正
    public float defence;

    // HP補正
    public float hp;

    // 効果時間
    public float duration;
}

//バフ・デバフを管理するマネージャークラス
public class BuffManager : MonoBehaviour
{
    [Header("現在かかっているバフ一覧")]
    [SerializeField]
    private List<Buff> buffs = new List<Buff>();

    //バフの追加
    public void AddBuff(Buff buff)
    {
        buffs.Add(buff);
    }

    //バフの経過時間更新
    private void Update()
    {
        for (int i = buffs.Count - 1; i >= 0; i--)
        {
            buffs[i].duration -= Time.deltaTime;

            if (buffs[i].duration <= 0)
            {
                buffs.RemoveAt(i);
            }
        }
    }

    //HPを増加させるバフ
    public float GetHPBuff()
    {
        float total = 0;

        foreach (Buff buff in buffs)
        {
            total += buff.hp;
        }

        return total;
    }

    //攻撃力を増加させるバフ
    public float GetAttackBuff()
    {
        float total = 0;

        foreach (Buff buff in buffs)
        {
            total += buff.attack;
        }

        return total;
    }

    //防御力を増加させるバフ
    public float GetDefenceBuff()
    {
        float total = 0;

        foreach (Buff buff in buffs)
        {
            total += buff.defence;
        }

        return total;
    }

}


