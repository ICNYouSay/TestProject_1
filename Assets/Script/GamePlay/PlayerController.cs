using Fusion;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : NetworkBehaviour
{
	[Header("移動設定")]
	public float moveSpeed = 5.0f;
	public float turnSpeed = 10.0f;

	[Header("モデルの向き補正")]
	public float modelRotationOffset = 0f;

	private CharacterController controller;
	private Animator anim;
	
	private NetworkCharacterController _ncc;
	private ICharacterLogic _currentLogic; // 現在のキャラロジック

	private bool uniqueSkillAInput = false;
	private bool uniqueSkillBInput = false;

    [Networked] public int SelectedCharacterID { get; set; }

	private Gamepad gamepad;

	void Start()
	{
	}

	public override void Spawned()
	{
		controller = GetComponent<CharacterController>();
		anim = GetComponentInChildren<Animator>();
		_ncc = GetComponent<NetworkCharacterController>();

		// キャラIDに応じてロジッククラスを生成してセットする（AddComponentはしない！）
		switch (SelectedCharacterID)
		{
			case 0: 
				_currentLogic = new CailLogic();
                _currentLogic.Initialize(GetComponentInChildren<Cail_Animation>());
                break;
			//case 1: _currentLogic = new MichaelLogic(); break;
		}

		// 見た目のモデルを Instantiate して子に配置
	}

	private void Update()
    {
        if (gamepad != null)
        {
            if (gamepad.buttonSouth.wasPressedThisFrame)
            {
				uniqueSkillAInput = true;
            }
            else if (gamepad.buttonEast.wasPressedThisFrame)
            {
				uniqueSkillBInput = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
				uniqueSkillAInput = true;
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
				uniqueSkillBInput = true;
            }
        }
    }

	public void PlayerWalk()
    {
        // 現在接続されているGamepadを取得
        gamepad = Gamepad.current;

        float moveX = 0.0f;
		float moveZ = 0.0f;
		Vector2 stickInput = Vector2.zero;

        if (gamepad == null)
        {
            // WASD入力
            moveX = Input.GetAxis("Horizontal");
            moveZ = Input.GetAxis("Vertical");
        }
        else
        {
	        // 左スティックの入力値を取得
		    stickInput = gamepad.leftStick.ReadValue();

        }

        // カメラ基準の方向計算
        Vector3 cameraForward = Camera.main.transform.forward;
		Vector3 cameraRight = Camera.main.transform.right;
		cameraForward.y = 0;
		cameraRight.y = 0;

		Vector3 inputDir = Vector3.zero;
		if (gamepad == null)
		{
			inputDir = (cameraForward.normalized * moveZ + cameraRight.normalized * moveX).normalized;
		}
		else
		{
			inputDir = (cameraForward.normalized * stickInput.y + cameraRight.normalized * stickInput.x).normalized;
		}
        Vector3 moveVelocity = Vector3.zero;

		if (inputDir.magnitude > 0.1f)
		{
			// 回転処理
			Quaternion targetRotation = Quaternion.LookRotation(inputDir);

			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Runner.DeltaTime);
			// 移動速度をセット
			moveVelocity = inputDir * moveSpeed;

			if (anim != null) anim.SetBool("isWalking", true);
		}
		else
		{
			if (anim != null) anim.SetBool("isWalking", false);
		}

		_currentLogic?.Move(_ncc, moveVelocity * Runner.DeltaTime);
	}


	public override void FixedUpdateNetwork()
	{
		// 入力権限がない場合は処理しない
		if (!Object.HasInputAuthority) return;


		if (uniqueSkillAInput)
		{
			_currentLogic?.UniqueSkillA(_ncc);
			uniqueSkillAInput = false; // 入力フラグをリセット
        }

		if (uniqueSkillBInput)
		{
			_currentLogic?.UniqueSkillB(_ncc);
			uniqueSkillBInput = false; // 入力フラグをリセット
        }

		PlayerWalk();
    }
}