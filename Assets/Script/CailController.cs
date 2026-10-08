using Fusion;
using UnityEngine;

public class CailController : PlayerController
{
    private CharacterController controller;
    private Animator anim;
    private NetworkCharacterController _ncc;

    // カイルのスキル判定用
    [Header("固有スキル用HitBox")]
    public SkillHitbox gungnirHitbox;

    // キー入力判定用フラグ
    private bool _GungnirPressed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Awake()
    {
        // このオブジェクトの子オブジェクトとして存在するモデルPrefabからNetworkCharacterControllerを取得
        _ncc = GetComponentInChildren<NetworkCharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _GungnirPressed = true;
        }
    }


    public override void FixedUpdateNetwork()
    {
        // if ((Input.GetKeyDown(KeyCode.E)) & (anim.GetInteger("Gungnir") == 0))
        if (_GungnirPressed)
        {
            if (anim.GetInteger("Gungnir") == 0)
            {
                anim.SetInteger("Gungnir", 1);
                // 攻撃開始
                if (gungnirHitbox != null) gungnirHitbox.EnableHitbox();
            }

            // 攻撃判定のOFF
            if (anim.GetInteger("Gungnir") == 2)
            {
                AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
                // アニメーションが1周完了したら判定をOFFにする
                if (stateInfo.normalizedTime >= 1.0f)
                {
                    if (gungnirHitbox != null) gungnirHitbox.DisableHitbox(); // OFFにする
                }
            }
        }
    }
}
