using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

//アクションごとのカメラ情報
[System.Serializable]
public class ActionCameraData
{
    [Header("アクション名")]
    public string actionName;

    [Header("Timeline")]
    public PlayableDirector timeline;

    [Header("Virtual Camera")]
    public CinemachineVirtualCamera virtualCamera;

    [Header("Dolly Cart")]
    public CinemachineDollyCart dollyCart;
}