using Cinemachine;
using Photon.Realtime;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //インスタンス
    public static CameraManager Instance;

    [Header("VirtualCameraPrefabを設定")]
    [SerializeField]
    private CinemachineVirtualCamera virtualCameraPrefab;

    [Header("ゲームプレイスタート用演出カメラ")]
    [SerializeField]
    private CinemachineVirtualCamera startCamera;

    //現在使用するVirtualCamera
    private CinemachineVirtualCamera currentCamera;

    //VirtualCameraのAim設定
    private CinemachineComposer composer;

    private void Awake()
    {
        //自身をInstanceとして登録
        Instance = this;
    }

    //カメラの追従対象を変更する
    public void SetTarget(Transform target)
    {
        // 古いVirtualCameraを削除
        if (currentCamera != null)
        {
            Destroy(currentCamera.gameObject);
        }

        // 新しいVirtualCamera生成
        currentCamera = Instantiate(virtualCameraPrefab);

        // CameraTarget取得
        Transform cameraTarget = target.Find("CameraTarget");

        foreach (Transform t in target.GetComponentsInChildren<Transform>(true))
        {
            if (t.name == "CameraTarget")
            {
                cameraTarget = t;
                break;
            }
        }

        // Follow・LookAt設定
        currentCamera.Follow = cameraTarget;
        currentCamera.LookAt = cameraTarget;

        // Third Person Follow取得
        Cinemachine3rdPersonFollow thirdPerson =
            currentCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();

        if (thirdPerson != null)
        {
            thirdPerson.ShoulderOffset = Vector3.zero;
            thirdPerson.VerticalArmLength = 3.0f;
            thirdPerson.CameraDistance = 8.0f;
        }

        // CameraControllerへ渡す
        CameraController controller = Camera.main.GetComponent<CameraController>();

        if (controller != null)
        {
            controller.SetVirtualCamera(currentCamera);
            controller.SetTarget(cameraTarget);
        }

    }

    //VirtualCameraの切り替え
    public void SwitchToEventCamera(CinemachineVirtualCamera eventCamera)
    {
        if (currentCamera != null)
        {
            currentCamera.Priority = 0;
        }

        eventCamera.Priority = 100;

        Camera.main.GetComponent<CameraController>()
                   .EnablePlayerControl(false);
    }

    public void ReturnPlayerCamera(CinemachineVirtualCamera eventCamera)
    {
        eventCamera.Priority = 0;

        if (currentCamera != null)
        {
            currentCamera.Priority = 100;
        }

        Camera.main.GetComponent<CameraController>()
                   .EnablePlayerControl(true);
    }

}
