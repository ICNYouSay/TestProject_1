using Fusion;
using UnityEditor.UI;
using UnityEngine;

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

    private void Update()
    {
    }

    public override void FixedUpdateNetwork()
    {
        // 入力権限がない場合は処理しない
        if (!Object.HasInputAuthority) return;

        // WASD入力
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // カメラ基準の方向計算
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;

        Vector3 inputDir = (cameraForward.normalized * moveZ + cameraRight.normalized * moveX).normalized;

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

        //通常攻撃
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (anim != null) anim.SetBool("isAttacking", true);
            Debug.Log("isAttacking=true");
        }
        else if (anim.GetBool("isAttacking"))
        {
            // 現在のAnimatorのステート情報を取得（0はベースレイヤー）
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            // normalizedTime が 1.0 以上なら、再生が1周（100%）完了している
            if (stateInfo.normalizedTime >= 1.0f)
            {
                anim.SetBool("isAttacking", false);
                Debug.Log("isAttacking=false");
            }
        }


        // NetworkCharacterControllerのMoveを使う
        _ncc.Move(moveVelocity * Runner.DeltaTime);
    }
}