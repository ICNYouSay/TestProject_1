using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

public class CailLogic : ICharacterLogic
{
	private CharacterController controller;
	private NetworkCharacterController _ncc;

	private Cail_Animetion cailAnim;

	// カイルのスキル判定用
	[Header("固有スキル用HitBox")]
	public SkillHitbox gungnirHitbox;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Initialize()
	{
		//controller = GetComponent<CharacterController>();
		// このオブジェクトの子オブジェクトとして存在するモデルPrefabからNetworkCharacterControllerを取得
		//_ncc = GetComponentInChildren<NetworkCharacterController>();
		//cailAnim = GetComponentInChildren<Cail_Animetion>();
	}

    // Update is called once per frame
    public void Update()
	{
		// 入力権限がない場合は処理しない
		//if (!Object.HasInputAuthority) return;

		if (Input.GetKeyDown(KeyCode.E) & cailAnim._GungnirPressed == false)
		{
			cailAnim.GungnirPressed();
		}
	}

	public void Move(NetworkCharacterController ncc, Vector3 inputVector)
	{
		ncc.Move(inputVector);
    }

	public void Destroy()
	{
		// キャラクターの破棄処理をここに記述
		//Destroy(gameObject);
    }
}
