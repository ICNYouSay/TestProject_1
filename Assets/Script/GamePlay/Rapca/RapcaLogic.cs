using Fusion;
using UnityEngine;

public class RapcaLogic : ICharacterLogic
{
    private Rapca_Animation RapcaAnim;
    public Animator _animator;

    public SkillHitbox timemagicHitbox;   // スキル1用
    public SkillHitbox skillBHitbox;       // スキル2用
    public SkillHitbox normalAttackHitbox; // 通常攻撃用

    // 通常攻撃（3段コンボ）用変数
    private int comboStep = 0;
    private float lastAttackTime = 0f;
    private float comboResetTime = 1.0f;

    // コンボごとのダメージ用変数
    private int currentAttackDamage = 50;

    public void Initialize(CharacterAnimation anim)
    {
        RapcaAnim = anim as Rapca_Animation;

        if (anim != null)
        {
            _animator = anim.GetComponent<Animator>();
            if (_animator == null) _animator = anim.GetComponentInChildren<Animator>();
            if (_animator == null) _animator = anim.transform.root.GetComponentInChildren<Animator>();

            SkillHitbox[] hitboxes = anim.transform.root.GetComponentsInChildren<SkillHitbox>(true);
            foreach (var hb in hitboxes)
            {
                // ヒットボックスの名前はプレハブに合わせて調整してください
                if (hb.gameObject.name == "Rapca_Timemagic_Hitbox") timemagicHitbox = hb;
                if (hb.gameObject.name == "Rapca_SkillB_Hitbox") skillBHitbox = hb;
                if (hb.gameObject.name == "Rapca_Normal_Hitbox") normalAttackHitbox = hb;
            }
        }
    }

    public void NormalAttack(NetworkCharacterController ncc)
    {
        if (Time.time - lastAttackTime > comboResetTime)
        {
            comboStep = 0;
        }

        lastAttackTime = Time.time;
        comboStep++;
        if (comboStep > 3) comboStep = 1;

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
                Debug.Log("【ラピカ通常 1段目】");
                if (normalAttackHitbox != null) normalAttackHitbox.TriggerHitbox(50, 0.2f, 0.2f);
                break;
            case 2:
                Debug.Log("【ラピカ通常 2段目】");
                if (normalAttackHitbox != null) normalAttackHitbox.TriggerHitbox(70, 0.2f, 0.2f);
                break;
            case 3:
                Debug.Log("【ラピカ通常 3段目フィニッシュ！】");
                if (normalAttackHitbox != null) normalAttackHitbox.TriggerHitbox(100, 0.2f, 0.3f);
                comboStep = 0;
                break;
        }
    }

    public void UniqueSkillA(NetworkCharacterController ncc)
    {
        ncc.Move(new Vector3(0, 0, 0));
        RapcaAnim.TimeMagicPressed();

        if (_animator != null)
        {
            _animator.SetTrigger("TimeMagic");
        }

        if (timemagicHitbox != null)
        {
            timemagicHitbox.TriggerHitbox(800, 3.0f, 1.0f);
        }

    }

    public void UniqueSkillB(NetworkCharacterController ncc)
    {
        
    }

    public void Move(NetworkCharacterController ncc, Vector3 inputVector)
    {
        ncc.Move(inputVector);
    }

    public void Destroy()
    {
    }
}