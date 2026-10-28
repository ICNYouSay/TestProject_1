using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

public class CailLogic : ICharacterLogic
{
    private Cail_Animation cailAnim;

    public SkillHitbox gungnirHitbox;

    // 通常攻撃（3段コンボ）用変数
    private int comboStep = 0;              // 現在のコンボ段数（0: 未攻撃, 1: 1段目, 2: 2段目, 3: 3段目）
    private float lastAttackTime = 0f;      // 最後に攻撃した時間
    private float comboResetTime = 1.0f;    // コンボが途切れて1段目に戻るまでの猶予時間（秒）

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Initialize(CharacterAnimation anim)
    {
        cailAnim = anim as Cail_Animation;

        // アニメーションがついているモデルの子供からSkillHitboxを自動取得する
        if (anim != null)
        {
            gungnirHitbox = anim.GetComponentInChildren<SkillHitbox>();
            if (gungnirHitbox == null)
            {
                gungnirHitbox = anim.transform.root.GetComponentInChildren<SkillHitbox>();
            }
        }

    }

    // 通常攻撃の処理
    public void NormalAttack(NetworkCharacterController ncc)
    {
        // 一定時間攻撃が空いたらコンボをリセット
        if (Time.time - lastAttackTime > comboResetTime)
        {
            comboStep = 0;
        }

        lastAttackTime = Time.time;
        comboStep++;

        // 3段目を超えたら1段目に戻す
        if (comboStep > 3) comboStep = 1;

        // 攻撃中は足を止める
        ncc.Move(new Vector3(0, 0, 0));

        switch (comboStep)
        {
            case 1:
                // cailAnim.NormalAttack1Pressed(); 
                // ※Cail_Animationのアニメーション名に合わせて呼び出す
                Debug.Log("【通常攻撃 1段目】");
                // 威力50、0.12秒後に発生、0.2秒間持続
                if (gungnirHitbox != null) gungnirHitbox.TriggerHitbox(50, 0.12f, 0.2f);
                break;

            case 2:
                // cailAnim.NormalAttack2Pressed(); 
                // ※Cail_Animationのアニメーション名に合わせて呼び出す
                Debug.Log("【通常攻撃 2段目】");
                // 威力70、0.12秒後に発生、0.2秒間持続
                if (gungnirHitbox != null) gungnirHitbox.TriggerHitbox(70, 0.12f, 0.2f);
                break;

            case 3:
                // cailAnim.NormalAttack3Pressed();
                // ※Cail_Animationのアニメーション名に合わせて呼び出す
                Debug.Log("【通常攻撃 3段目フィニッシュ！】");
                // 威力150、0.2秒後に発生、0.3秒間持続
                if (gungnirHitbox != null) gungnirHitbox.TriggerHitbox(150, 0.2f, 0.3f);
                comboStep = 0; // 3段目を撃ち切ったら最初に戻す
                break;
        }
    }

    // Update is called once per frame
    public void UniqueSkillA(NetworkCharacterController ncc)
    {
        cailAnim.GungnirPressed();
        ncc.Move(new Vector3(0, 0, 0));

        // TriggerHitboxを呼び出す
        if (gungnirHitbox != null)
        {
            gungnirHitbox.TriggerHitbox();
        }

    }

    public void UniqueSkillB(NetworkCharacterController ncc)
    {
        cailAnim.YburnPressed();
    }

    public void Move(NetworkCharacterController ncc, Vector3 inputVector)
    {
        ncc.Move(inputVector);
    }

    public void Destroy()
    {
    }
}