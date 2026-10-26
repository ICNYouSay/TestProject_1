using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

public class MichaelLogic : ICharacterLogic
{
	private Michael_Animation cailAnim;

	public SkillHitbox gungnirHitbox;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	public void Initialize(CharacterAnimation anim)
	{
		cailAnim = anim as Michael_Animation;
	}

	// Update is called once per frame
	public void UniqueSkillA(NetworkCharacterController ncc)
	{
		cailAnim.GungnirPressed();
		ncc.Move(new Vector3(0, 0, 0));
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
