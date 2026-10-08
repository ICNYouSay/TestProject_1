using Fusion;
using UnityEngine;

public class CailController : PlayerController
{
    private CharacterController controller;
    private NetworkCharacterController _ncc;

    private Cail_Animetion cailAnim;

    // カイルのスキル判定用
    [Header("固有スキル用HitBox")]
    public SkillHitbox gungnirHitbox;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Awake()
    {
        // このオブジェクトの子オブジェクトとして存在するモデルPrefabからNetworkCharacterControllerを取得
        _ncc = GetComponentInChildren<NetworkCharacterController>();
        cailAnim = GetComponentInChildren<Cail_Animetion>();
    }

    // Update is called once per frame
    void Update()
    {
        // 入力権限がない場合は処理しない
        if (!Object.HasInputAuthority) return;

        if (Input.GetKeyDown(KeyCode.E) & cailAnim._GungnirPressed == false)
        {
            cailAnim.GungnirPressed();
        }
    }


    public override void FixedUpdateNetwork()
    {
        // 入力権限がない場合は処理しない
        if (!Object.HasInputAuthority) return;

        if (cailAnim._GungnirPressed == false)
        {
            PlayerWalk();
        }
        /*
        if (_GungnirPressed)
        {
            // 攻撃開始
            if (gungnirHitbox != null) gungnirHitbox.EnableHitbox();
            
            // 攻撃判定のOFF
            // アニメーションが1周完了したら判定をOFFにする
            if (stateInfo.normalizedTime >= 1.0f)
            {
                if (gungnirHitbox != null) gungnirHitbox.DisableHitbox(); // OFFにする
            }
        }
        */
    }
}
