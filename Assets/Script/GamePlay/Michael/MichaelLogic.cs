using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

public class MichaelLogic : ICharacterLogic
{
    private Michael_Animation MichaelAnim;
    public Animator _animator;

    // マイケル用のヒットボックス
    public SkillHitbox normalAttackHitbox; // 通常攻撃
    public SkillHitbox straightHitbox;     // 固有1
    public SkillHitbox kostarpunchHitbox;  // 固有2

    // マイケル用の通常攻撃（2段コンボ）変数
    private int comboStep = 0;
    private float lastAttackTime = 0f;
    private float comboResetTime = 1.0f;

    public void Initialize(CharacterAnimation anim)
    {
        MichaelAnim = anim as Michael_Animation;

        // アニメーターとヒットボックスの自動取得
        if (anim != null)
        {
            _animator = anim.GetComponent<Animator>();
            if (_animator == null) _animator = anim.GetComponentInChildren<Animator>();
            if (_animator == null) _animator = anim.transform.root.GetComponentInChildren<Animator>();

            SkillHitbox[] hitboxes = anim.transform.root.GetComponentsInChildren<SkillHitbox>(true);
            foreach (var hb in hitboxes)
            {
                if (hb.gameObject.name == "Michael_Normal_Hitbox") normalAttackHitbox = hb;
                if (hb.gameObject.name == "Michael_Straight_Hitbox") straightHitbox = hb;
                if (hb.gameObject.name == "Michael_KOStarPunch_Hitbox") kostarpunchHitbox = hb;
            }
        }
    }

    // マイケルの通常攻撃（2段パンチ）
    public void NormalAttack(NetworkCharacterController ncc)
    {
        if (Time.time - lastAttackTime > comboResetTime)
        {
            comboStep = 0;
        }

        lastAttackTime = Time.time;
        comboStep++;
        if (comboStep > 2) comboStep = 1; // 2段でループ

        ncc.Move(new Vector3(0, 0, 0));

        // AnimatorのAttackトリガーを引く
        if (_animator != null)
        {
            _animator.SetTrigger("Attack"); 
        }

        // 段数に応じた当たり判定を出す
        switch (comboStep)
        {
            case 1:
                Debug.Log("【マイケル通常 1段目】");
                if (normalAttackHitbox != null) normalAttackHitbox.TriggerHitbox(60, 0.1f, 0.2f);
                break;
            case 2:
                Debug.Log("【マイケル通常 2段目フィニッシュ！】");
                if (normalAttackHitbox != null) normalAttackHitbox.TriggerHitbox(90, 0.15f, 0.25f);
                comboStep = 0;
                break;
        }
    }

    // 特殊技Aの処理
    public void UniqueSkillA(NetworkCharacterController ncc)
    {
        Debug.Log("【マイケル特殊技】G.O.ストレート！");

        MichaelAnim.StraightPressed();
        ncc.Move(new Vector3(0, 0, 0));

        // Animator側の「GOStraight」トリガーを引く
        if (_animator != null)
        {
            _animator.SetTrigger("GOStraight"); 
        }

        // 特殊技の当たり判定を出す
        if (straightHitbox != null)
        {
            straightHitbox.TriggerHitbox(600, 2.75f, 0.4f);
        }

    }

    // 特殊技Bの処理
    public void UniqueSkillB(NetworkCharacterController ncc)
    {
        Debug.Log("【マイケル特殊技】K.O.スターパンチ！");

        // Animatorのトリガーを引く
        if (_animator != null)
        {
            _animator.SetTrigger("KOStarPunch");
        }

        ncc.Move(new Vector3(0, 0, 0));

        // 特殊技の当たり判定を出す
        if (kostarpunchHitbox != null)
        {
            kostarpunchHitbox.TriggerHitbox(1200, 3.0f, 0.5f);
        }
    }

    public void Move(NetworkCharacterController ncc, Vector3 inputVector)
    {
        ncc.Move(inputVector);
    }

    public void Destroy()
    {
    }
}