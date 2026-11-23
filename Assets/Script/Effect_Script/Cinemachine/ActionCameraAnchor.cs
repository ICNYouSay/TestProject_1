using UnityEngine;
using Cinemachine;

// プレイヤーが持つアクション用Path
public class ActionCameraAnchor : MonoBehaviour
{
    [SerializeField]
    private ActionPathData[] actionPaths;

    public CinemachineSmoothPath GetPath(string actionName)
    {
        foreach (var data in actionPaths)
        {
            if (data.actionName == actionName)
            {
                return data.path;
            }
        }

        Debug.LogWarning($"Pathが見つかりません : {actionName}");
        return null;
    }
}