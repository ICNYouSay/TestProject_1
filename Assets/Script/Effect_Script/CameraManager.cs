using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [Header("VirtualCameraPrefabを設定")]
    [SerializeField] private CinemachineVirtualCamera virtualCameraPrefab;

    [Header("ゲームプレイスタート用演出カメラ")]
    [SerializeField] private CinemachineVirtualCamera startCamera;

    private CinemachineVirtualCamera currentCamera;
    private CinemachineComposer composer;

    private void Awake()
    {
        Instance = this;
    }

    public void SetTarget(Transform target)
    {
        if (currentCamera != null)
        {
            Destroy(currentCamera.gameObject);
        }

        currentCamera = Instantiate(virtualCameraPrefab);

        Transform cameraTarget = target.Find("CameraTarget");

        foreach (Transform t in target.GetComponentsInChildren<Transform>(true))
        {
            if (t.name == "CameraTarget")
            {
                cameraTarget = t;
                break;
            }
        }

        currentCamera.Follow = cameraTarget;
        currentCamera.LookAt = cameraTarget;

        Cinemachine3rdPersonFollow thirdPerson =
            currentCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();

        if (thirdPerson != null)
        {
            thirdPerson.ShoulderOffset = Vector3.zero;
            thirdPerson.VerticalArmLength = 3.0f;
            thirdPerson.CameraDistance = 8.0f;
        }

        CameraController controller = Camera.main.GetComponent<CameraController>();

        if (controller != null)
        {
            controller.SetVirtualCamera(currentCamera);
            controller.SetTarget(cameraTarget);
        }
    }

    //プレイヤーカメラからイベントカメラに切り替える
    public void SwitchToEventCamera(CinemachineVirtualCamera eventCamera, Transform target)
    {
        if (currentCamera != null)
        {
            currentCamera.Priority = 0;
        }
        eventCamera.Follow = target;
        eventCamera.LookAt = target;
        eventCamera.Priority = 100;
        Camera.main.GetComponent<CameraController>()
                   .EnablePlayerControl(false);
    }

    // イベントカメラからプレイヤーカメラに戻す
    public void ReturnToPlayerCamera()
    {
        if (currentCamera != null)
        {
            currentCamera.Priority = 100;
        }
        Camera.main.GetComponent<CameraController>()
                   .EnablePlayerControl(true);
    }
}