using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

public class CailLogic : ICharacterLogic
{
	private Cail_Animation cailAnim;

	public SkillHitbox gungnirHitbox;

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
