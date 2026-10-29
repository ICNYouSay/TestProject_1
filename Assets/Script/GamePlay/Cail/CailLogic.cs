using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

public class CailLogic : ICharacterLogic
{
    private Cail_Animation cailAnim;
    private Animator _animator;

    public SkillHitbox gungnirHitbox;
    public SkillHitbox normalAttackHitbox;

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
            _animator = anim.GetComponent<Animator>();
            if (_animator == null) _animator = anim.GetComponentInChildren<Animator>();
            if (_animator == null) _animator = anim.transform.root.GetComponentInChildren<Animator>();

            SkillHitbox[] hitboxes = anim.transform.root.GetComponentsInChildren<SkillHitbox>(true);
            foreach (var hb in hitboxes)
            {
                if (hb.gameObject.name == "Skill_Hitbox") gungnirHitbox = hb;
                if (hb.gameObject.name == "Cail_Normal_Hitbox") normalAttackHitbox = hb;
            }
        }

    }

    // 通常攻撃の処理
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

        // Triggerを鳴らすだけ
        if (_animator != null)
        {
            _animator.SetTrigger("Attack");
        }

        // 段数に応じた当たり判定を出す
        switch (comboStep)
        {
            case 1:
                Debug.Log("【通常攻撃 1段目】");
                if (normalAttackHitbox != null) normalAttackHitbox.TriggerHitbox(50, 0.12f, 0.2f);
                break;
            case 2:
                Debug.Log("【通常攻撃 2段目】");
                if (normalAttackHitbox != null) normalAttackHitbox.TriggerHitbox(70, 0.12f, 0.2f);
                break;
            case 3:
                Debug.Log("【通常攻撃 3段目フィニッシュ！】");
                if (normalAttackHitbox != null) normalAttackHitbox.TriggerHitbox(150, 0.2f, 0.3f);
                comboStep = 0;
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