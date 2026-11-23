using UnityEngine;
using UnityEngine.Playables;

//特殊アクション用カメラ管理
public class ActionCamerawork : MonoBehaviour
{
    //キャラクターごとのカメラ情報
    [SerializeField]
    private CharacterActionCamera[] characterCameras;

    //現在再生中
    private ActionCameraData currentAction;

    //シングルトン
    public static ActionCamerawork Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    //アクションカメラ再生
    public void PlayActionCamera(int characterID, string actionName,
        ActionCameraAnchor anchor, Transform cameraTarget)
    {
        foreach (var character in characterCameras)
        {
            if (character.characterID != characterID)
                continue;

            foreach (var action in character.actions)
            {
                if (action.actionName != actionName)
                    continue;

                //VirtualCameraのFollowとLookAtを設定
                //action.virtualCamera.Follow = cameraTarget;
                action.virtualCamera.LookAt = cameraTarget;

                // プレイヤーからSmoothPathを取得
                if (anchor != null)
                {
                    Cinemachine.CinemachineSmoothPath path = anchor.GetPath(actionName);

                    if (path != null && action.dollyCart != null)
                    {
                        // DollyCartのPathを変更
                        action.dollyCart.m_Path = path;
                    }
                }

                currentAction = action;

                //Dolly初期化
                if (action.dollyCart != null)
                {
                    action.dollyCart.m_Position = 0;
                }

                //VirtualCameraを有効
                if (action.virtualCamera != null)
                {
                    action.virtualCamera.Priority = 100;
                }

                //Timelineを最初から再生
                if (action.timeline != null)
                {
                    action.timeline.time = 0;
                    action.timeline.Evaluate();
                    action.timeline.Play();
                }

                return;
            }
        }

        Debug.LogWarning($"Camera Action Not Found Character:{characterID} Action:{actionName}");
    }

    //Timeline終了
    public void OnActionCameraFinished()
    {
        //現在のアクションカメラがなければ即時終了
        if (currentAction == null)
            return;

        //現在にアクションカメラがあれば停止
        if (currentAction.timeline != null)
        {
            //Timelineを停止
            currentAction.timeline.Stop();
        }

        //VirtualCameraを無効
        if (currentAction.virtualCamera != null)
        {
            //VirtualCameraの優先度を下げる
            currentAction.virtualCamera.Priority = 0;
        }

        //DollyCartを初期化
        if (currentAction.dollyCart != null)
        {
            //DollyCartの位置を初期化
            currentAction.dollyCart.m_Position = 0;
        }

        currentAction = null;
    }
}