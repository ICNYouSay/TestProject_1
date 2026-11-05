using UnityEngine;
using Cinemachine;

public class CinemaTrackController : MonoBehaviour
{
    // Virtual Camera と Path をインスペクターで設定できるようにする
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private CinemachinePathBase _path;

    // Tracked Dolly コンポーネントへの参照
    private CinemachineTrackedDolly _dolly;

    private void Awake()
    {
        if (_virtualCamera == null)
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }

        _dolly = _virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();

        if (_dolly == null)
        {
            Debug.LogError("Tracked Dolly が Virtual Camera に設定されていません。");
            enabled = false;
            return;
        }

        if (_path != null)
        {
            _dolly.m_Path = _path;
        }
    }

    // 0.0 ～ 1.0 で進めたい場合
    public void SetNormalizedPosition(float t)
    {
        _dolly.m_PositionUnits = CinemachinePathBase.PositionUnits.Normalized;
        _dolly.m_PathPosition = Mathf.Clamp01(t);
    }

    // 距離で進めたい場合
    public void SetDistancePosition(float distance)
    {
        _dolly.m_PositionUnits = CinemachinePathBase.PositionUnits.Distance;
        _dolly.m_PathPosition = Mathf.Max(0f, distance);
    }

    // ウェイポイント基準で進めたい場合
    public void SetPathUnitPosition(float unit)
    {
        _dolly.m_PositionUnits = CinemachinePathBase.PositionUnits.PathUnits;
        _dolly.m_PathPosition = Mathf.Max(0f, unit);
    }

    // 自動追従に切り替える
    public void EnableAutoDolly(Transform followTarget, float offset = 0f)
    {
        _virtualCamera.Follow = followTarget;
        _dolly.m_AutoDolly = new CinemachineTrackedDolly.AutoDolly(true, offset, 4, 4);
    }

    public void DisableAutoDolly()
    {
        _dolly.m_AutoDolly = new CinemachineTrackedDolly.AutoDolly(false, 0f, 4, 4);
    }
}