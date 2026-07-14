using Fusion;
using UnityEngine;

// [RequireComponent] を書くと、このスクリプトを入れた時に必要な部品を自動で追加してくれます
[RequireComponent(typeof(CharacterController))]
public class PlayerController : NetworkBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 25.0f;     // キャラクターの移動速度
    public float turnSpeed = 10.0f;    // キャラクターが回転する時の速さ
    public float gravity = 9.8f;       // 地面に向かって引っ張る重力の強さ

    [Header("モデルの向き補正")]
    [Tooltip("Blenderモデルの正面がズレている場合、ここで角度(0, 90, 180など)を調整します")]
    public float modelRotationOffset = 0f;

    // 内部で使用するコンポーネントの参照用変数
    private CharacterController controller; // 移動を制御するコンポーネント
    private Animator anim;                // アニメーションを制御するコンポーネント

    // ゲーム開始時に一度だけ呼ばれる
    void Start()
    {
        // 自分のオブジェクトに付いている CharacterController を取得
        controller = GetComponent<CharacterController>();

        // 子オブジェクト（Cail_Walkなど）に付いている Animator を探して取得
        anim = GetComponentInChildren<Animator>();
    }

    // 普通のCharacterControllerではなく、Fusion専用の方をキャッシュする
    private NetworkCharacterController _ncc;
    
    private void Awake()
    {
        _ncc = GetComponent<NetworkCharacterController>();
    }

    // 1フレームごとに呼ばれる
    public override void FixedUpdateNetwork()
    {
        Debug.Log($"FUN動作中 権限の有無: {HasStateAuthority}");
        if (HasStateAuthority)
        if (!Object.HasInputAuthority)
        return;


        {
            // WASDキー（または矢印キー）の入力を -1.0 ～ 1.0 の間で取得
            float moveX = Input.GetAxis("Horizontal"); // A/D または 左/右
            float moveZ = Input.GetAxis("Vertical");   // W/S または 上/下

            // カメラの向きを基準に移動方向を計算する
            // カメラの「前」と「右」がどっちを向いているか取得
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;

            // キャラは上下には進まないので、Y軸（高さ）の成分は 0 にして水平にする
            cameraForward.y = 0;
            cameraRight.y = 0;

            // 入力された値とカメラの向きを組み合わせて、最終的な「進みたい方向」を決定
            Vector3 inputDir = (cameraForward.normalized * moveZ + cameraRight.normalized * moveX);

            // 入力がある時（キーが押されている時）の処理
            if (inputDir.magnitude > 0.1f)
            {
                // 回転の処理
                // 進みたい方向（inputDir）を向くための回転を作成
                Quaternion targetRotation = Quaternion.LookRotation(inputDir);

                // モデル固有の向きのズレ（Offset）を補正用の回転として作成
                Quaternion offsetRotation = Quaternion.Euler(0, modelRotationOffset, 0);

                // 補正を加えた最終的な向きに向かって、滑らかに回転させる
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation * offsetRotation, turnSpeed * Time.deltaTime);

                // 移動の処理
                // CharacterControllerを使って、実際にキャラクターを動かす
                _ncc.Move(inputDir * moveSpeed * Time.deltaTime);

                // アニメーションの処理
                // アニメーターの "isWalking" スイッチを true (ON) にする
                if (anim != null) anim.SetBool("isWalking", true);
            }
            else
            {
                // 入力がない（キーを離している）時の処理
                // アニメーターの "isWalking" スイッチを false (OFF) にして待機状態に戻す
                if (anim != null) anim.SetBool("isWalking", false);
            }

            // 重力の処理
            // もし地面に接地していなければ（空中にいれば）
            if (!controller.isGrounded)
            {
                // 常に下方向（Vector3.down）に重力分だけ移動させる
                _ncc.Move(Vector3.down * gravity * Time.deltaTime);
            }
        }
    }
}