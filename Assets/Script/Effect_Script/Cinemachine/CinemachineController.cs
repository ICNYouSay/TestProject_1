using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

// Cinemachineのカメラ管理を行うシングルトンコントローラークラス
public class CinemachineController : MonoBehaviour
{
    // シングルトンインスタンス
    public static CinemachineController Instance { get; private set; }

    // カメラエントリークラス
    [System.Serializable]
    public class CameraEntry
    {
        public string cameraName;
        public CinemachineVirtualCamera virtualCamera;
    }

    // カメラリストと優先度設定
    [Header("Cinemachine Cameras")]
    [SerializeField] private List<CameraEntry> cameras = new List<CameraEntry>();

    // カメラの優先度設定
    [Header("Priority Settings")]
    [SerializeField] private int activePriority = 20;
    [SerializeField] private int inactivePriority = 0;

    // カメラ名と仮想カメラの辞書
    private Dictionary<string, CinemachineVirtualCamera> cameraDictionary;

    // シングルトンの初期化とカメラ辞書の構築
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // 必要ならシーン遷移でも残す
        // DontDestroyOnLoad(gameObject);

        cameraDictionary = new Dictionary<string, CinemachineVirtualCamera>();

        foreach (var entry in cameras)
        {
            if (entry == null || entry.virtualCamera == null) continue;
            if (string.IsNullOrEmpty(entry.cameraName)) continue;

            if (!cameraDictionary.ContainsKey(entry.cameraName))
            {
                cameraDictionary.Add(entry.cameraName, entry.virtualCamera);
            }
        }
    }

    public void SetCamera(string cameraName)
    {
        if (!cameraDictionary.ContainsKey(cameraName))
        {
            Debug.LogWarning($"Camera '{cameraName}' が見つかりません。");
            return;
        }

        foreach (var cam in cameraDictionary.Values)
        {
            if (cam != null)
            {
                cam.Priority = inactivePriority;
            }
        }

        cameraDictionary[cameraName].Priority = activePriority;
    }

    public void SetCamera(CinemachineVirtualCamera targetCamera)
    {
        if (targetCamera == null) return;

        foreach (var entry in cameraDictionary.Values)
        {
            if (entry != null)
            {
                entry.Priority = inactivePriority;
            }
        }

        targetCamera.Priority = activePriority;
    }
}