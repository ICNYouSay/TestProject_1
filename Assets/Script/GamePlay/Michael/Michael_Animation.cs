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
        Debug.Log("G.O.ストレート発動！");
        _StraightPressed = true;
    }

	public void StraightJumped()
	{

	}

	public void StraightAttacked()
	{

	}

	public void YburnPressed()
	{ 		

    }
}