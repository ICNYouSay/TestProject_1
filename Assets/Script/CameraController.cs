using UnityEngine;

public class CameraController : MonoBehaviour
{
    // 追いかける対象
    public Transform target;

    // 感度
    public float mouseSensitivity = 2.0f;

    private float rotationX = 0f;
    private float rotationY = 0f;

    // キャラからの距離
    public float distance = 10.0f;
    // カメラの高さ
    public float height = 3.0f;     

    void Start()
    {
        // マウスカーソルを画面中央で固定して消す
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (target == null) return;

        // マウスの入力を取得
        rotationX += Input.GetAxis("Mouse X") * mouseSensitivity;
        rotationY -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        // 上下の視界制限
        rotationY = Mathf.Clamp(rotationY, -20f, 50f);

        // 回転を作成（上下左右の両方を含める）
        Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0);

        // 注視点を計算
        // heightはカメラの高さではなくキャラのどの高さを中心にするか
        Vector3 lookAtPoint = target.position + Vector3.up * height;

        // カメラの位置を計算
        //（注視点から回転方向の後ろにdistance分下がる）
        Vector3 position = lookAtPoint + (rotation * Vector3.back * distance);

        // 反映
        transform.position = position;
        transform.rotation = rotation;
    }

}