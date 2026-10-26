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
	}

	// Update is called once per frame
	public void UniqueSkillA()
	{
		cailAnim.GungnirPressed();
	}

	public void UniqueSkillB()
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
