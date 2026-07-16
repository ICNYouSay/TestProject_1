using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    // カメラが追従する対象（プレイヤーキャラクターの子オブジェクト CameraTarget）
    private Transform target;

    // 操作するVirtualCamera
    private CinemachineVirtualCamera virtualCamera;

    [Header("マウス設定")]

    // マウスの左右移動の感度
    [SerializeField]
    private float mouseSensitivity = 150f;

    [Header("カメラ設定")]

    [Header("どれだけ後ろに離すか")]
    // CameraTargetからどれだけ後ろに離すか
    [SerializeField]
    private float distance = 8f;

    [Header("どれだけ上に配置するか")]
    // CameraTargetからどれだけ上に配置するか
    [SerializeField]
    private float height = 2f;

    [Header("どれだけの速度で追従するか")]
    // カメラが追従する速度
    // 値が大きいほど素早く追従します
    [SerializeField]
    private float followSpeed = 10f;

    // カメラの左右回転角度
    private float yaw = 0f;

    private void Start()
    {
        // マウスカーソルを画面中央に固定
        Cursor.lockState = CursorLockMode.Locked;

        // マウスカーソルを非表示
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        // ターゲットまたはVirtualCameraが設定されていなければ何もしない
        if (target == null || virtualCamera == null)
            return;

        //--------------------------------------------------
        // マウスの左右入力を取得
        //--------------------------------------------------
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        //--------------------------------------------------
        // カメラの回転を作成
        // Y軸のみ回転させる
        //--------------------------------------------------
        Quaternion rotation = Quaternion.Euler(0f, yaw, 0f);

        //--------------------------------------------------
        // CameraTargetを中心とした注視位置を計算
        //--------------------------------------------------
        Vector3 lookPoint = target.position + Vector3.up * height;

        //--------------------------------------------------
        // カメラをプレイヤーの後方へ配置
        //--------------------------------------------------
        Vector3 desiredPosition =
            lookPoint + rotation * new Vector3(0f, 0f, -distance);

        //--------------------------------------------------
        // カメラ位置を滑らかに移動
        //--------------------------------------------------
        virtualCamera.transform.position =
            Vector3.Lerp(
                virtualCamera.transform.position,
                desiredPosition,
                followSpeed * Time.deltaTime);

        //--------------------------------------------------
        // カメラをプレイヤーへ向ける
        //--------------------------------------------------
        virtualCamera.transform.LookAt(lookPoint);
    }

    /// カメラが追従する対象(CameraTarget)を設定
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    /// 操作対象となるVirtualCameraを設定
    public void SetVirtualCamera(CinemachineVirtualCamera cam)
    {
        virtualCamera = cam;
    }
}