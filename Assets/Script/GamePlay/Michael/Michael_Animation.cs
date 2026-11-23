using Fusion;
using UnityEngine;

public class Michael_Animation : CharacterAnimation
{
	private CharacterController controller;
	private Animator anim;
	private NetworkCharacterController _ncc;


	// キー入力判定用フラグ
	public bool _StraightPressed;
	public bool _YburnPressed;

    void Start()
	{
		controller = GetComponentInParent<CharacterController>();
		anim = GetComponent<Animator>();
		_ncc = GetComponentInParent<NetworkCharacterController>();
	}

	private void Update()
	{
	}

	public void StraightPressed()
	{
		if (anim.GetInteger("Straight") == 0)
		{
			anim.SetInteger("Straight", 1);
			Debug.Log("Straight=1");
			_StraightPressed = true;
		}
	}

	public void StraightJumped()
	{
		anim.SetInteger("Straight", 2);
		Debug.Log("Straight=2");
		_ncc.Move(new Vector3(0.0f, 0.0f, 5.0f));
	}

	public void StraightAttacked()
	{
		anim.SetInteger("Straight", 0);
		Debug.Log("Straight=0");
		_StraightPressed = false;
	}

	public void YburnPressed()
	{ 		
		if (anim.GetInteger("Yburn") == 0)
		{
			anim.SetInteger("Yburn", 1);
			Debug.Log("Yburn=1");
			_YburnPressed = true;
        }
    }
}