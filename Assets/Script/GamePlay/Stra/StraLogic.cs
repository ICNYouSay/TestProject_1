using Fusion;
using UnityEngine;

public class StraLogic : ICharacterLogic
{
    private Stra_Animation straAnim;
    public Animator _animator;

    public SkillHitbox normalAttackHitbox;
    public SkillHitbox skillAHitbox;

    public void Initialize(CharacterAnimation anim)
    {
        straAnim = anim as Stra_Animation;

        if (anim != null)
        {
            _animator = anim.GetComponent<Animator>();
            if (_animator == null) _animator = anim.GetComponentInChildren<Animator>();
            if (_animator == null) _animator = anim.transform.root.GetComponentInChildren<Animator>();

            SkillHitbox[] hitboxes = anim.transform.root.GetComponentsInChildren<SkillHitbox>(true);
            foreach (var hb in hitboxes)
            {
                if (hb.gameObject.name == "Stra_Normal_Hitbox") normalAttackHitbox = hb;
                if (hb.gameObject.name == "Stra_SkillA_Hitbox") skillAHitbox = hb;
            }
        }
    }

    public void NormalAttack(NetworkCharacterController ncc)
    {
        Debug.Log("ÅyÉXÉgÉâí èÌçUåÇÅz");
        ncc.Move(new Vector3(0, 0, 0));

        if (_animator != null)
        {
            _animator.SetTrigger("Attack");
        }

        if (normalAttackHitbox != null)
        {
            normalAttackHitbox.TriggerHitbox(50, 0.1f, 0.2f);
        }
    }

    public void UniqueSkillA(NetworkCharacterController ncc)
    {
        Debug.Log("ÅyÉXÉgÉâì¡éÍãZAÅz");
        ncc.Move(new Vector3(0, 0, 0));

        if (skillAHitbox != null)
        {
            skillAHitbox.TriggerHitbox(100, 0.2f, 0.3f);
        }
    }

    public void UniqueSkillB(NetworkCharacterController ncc)
    {
        Debug.Log("ÅyÉXÉgÉâì¡éÍãZBÅz");
    }

    public void Move(NetworkCharacterController ncc, Vector3 inputVector)
    {
        ncc.Move(inputVector);
    }

    public void Destroy()
    {
    }
}