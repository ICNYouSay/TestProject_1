using Fusion;
using UnityEngine;

public class Cail_Animation : CharacterAnimation
{
	private CharacterController controller;
	private Animator anim;
	private NetworkCharacterController _ncc;


	// キー入力判定用フラグ
	public bool _GungnirPressed;
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

	public void NormalAttacked()
    {
        anim.SetBool("CanWalk", true);
    }

    public void GungnirPressed()
	{
		if (anim.GetInteger("Gungnir") == 0)
		{
			anim.SetInteger("Gungnir", 1);
			Debug.Log("Gungnir=1");
			_GungnirPressed = true;
		}
	}

	public void GungnirJumped()
	{
        anim.SetInteger("Gungnir", 2);

        Debug.Log("Gungnir=2");
	}

	public void GungnirAttacked()
	{
		anim.SetInteger("Gungnir", 0);
		Debug.Log("Gungnir=0");
		_GungnirPressed = false;
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

    public void YburnJumped()
    {
        anim.SetInteger("Yburn", 2);
        Debug.Log("Yburn=2");
    }

    public void YburnAttacked()
    {
        anim.SetInteger("Yburn", 0);
        Debug.Log("Yburn=0");
        _YburnPressed = false;
    }

}