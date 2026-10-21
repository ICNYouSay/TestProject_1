using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

public class CailLogic : ICharacterLogic
{
	private Cail_Animetion cailAnim;

	public SkillHitbox gungnirHitbox;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	public void Initialize()
	{
		GameObject cailModelPrefab = Resources.Load<GameObject>("Prefab/Cail");
		GameObject cailModelInstance = GameObject.Instantiate(cailModelPrefab);
		cailAnim = cailModelInstance.GetComponent<Cail_Animetion>();
	}

	// Update is called once per frame
	public void Update()
	{
		if (GungnirKeyCheck())
		{
			cailAnim.GungnirPressed();
		}
	}

	bool GungnirKeyCheck()
	{
		if (cailAnim._GungnirPressed == false) return false;

		if (Input.GetKeyDown(KeyCode.E) & cailAnim._GungnirPressed == false)
		{
			return true;
		}
		else
		{
			return false;
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
