using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    //インスタンス
    public static CameraManager Instance;

    [Header("VirtualCameraPrefabを設定")]
    [SerializeField]
    private CinemachineVirtualCamera virtualCameraPrefab;

    private CinemachineVirtualCamera currentCamera;

    //VirtualCameraのAim設定
    private CinemachineComposer composer;

    private void Awake()
    {
        //自身をInstanceとして登録
        Instance = this;

        //CinemachineComposer取得
        composer = currentCamera.GetCinemachineComponent<CinemachineComposer>();
    }

    //カメラの追従対象を変更する
    public void SetTarget(Transform target)
    {
        Debug.Log($"SetTarget : {target.name}");

        //既にカメラがあるなら削除
        if (currentCamera != null)
        {
            Debug.Log("古いVirtualCameraを削除");
            Destroy(currentCamera.gameObject);
        }

        //新しいVirtualCameraを生成
        currentCamera = Instantiate(virtualCameraPrefab);

        Debug.Log($"生成したVirtualCamera : {currentCamera.name}");

        //プレイヤーを追従
        currentCamera.Follow = target;
        currentCamera.LookAt = target;

        Debug.Log($"Follow = {currentCamera.Follow.name}");

        // Body(Transposer)を取得
        var transposer = currentCamera.GetCinemachineComponent<CinemachineTransposer>();

        if (transposer != null)
        {
            transposer.m_FollowOffset = new Vector3(0f, 5f, -10f);
        }

        
    }

}
