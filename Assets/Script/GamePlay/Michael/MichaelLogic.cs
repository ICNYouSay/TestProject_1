using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

public class MichaelLogic : ICharacterLogic
{
	private Michael_Animation MichaelAnim;
    public Animator _animator;

    public SkillHitbox gungnirHitbox;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	public void Initialize(CharacterAnimation anim)
	{
		MichaelAnim = anim as Michael_Animation;

        // アニメーターを取得する
        if (anim != null)
        {
            _animator = anim.GetComponent<Animator>();
            if (_animator == null) _animator = anim.GetComponentInChildren<Animator>();
            if (_animator == null) _animator = anim.transform.root.GetComponentInChildren<Animator>();
        }
    }

    public void NormalAttack(NetworkCharacterController ncc)
    {
        // 攻撃トリガーを引く
        if (_animator != null)
        {
            _animator.SetTrigger("Attack");
        }
    }

    // Update is called once per frame
    public void UniqueSkillA(NetworkCharacterController ncc)
	{
        Debug.Log("UniqueSkillA");
        MichaelAnim.StraightPressed();
		ncc.Move(new Vector3(0, 0, 0));
    }

	public void UniqueSkillB(NetworkCharacterController ncc)
	{
		MichaelAnim.YburnPressed();
	}

	public void Move(NetworkCharacterController ncc, Vector3 inputVector)
	{
		ncc.Move(inputVector);
	}

	public void Destroy()
	{
	}
}
