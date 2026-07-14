using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    //インスタンス
    public static CameraManager Instance;

    [Header("シーンに配置したCinemachine Camera")]
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        //自身をInstanceとして登録
        Instance = this;
    }

    //カメラの追従対象を変更する
    public void SetTarget(Transform target)
    {
        //追従対象
        virtualCamera.Follow = target;

        //注視対象
        virtualCamera.LookAt = target;
    }

}
