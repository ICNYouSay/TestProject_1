using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline
{
    //Timelinde使用するShakeクリップ
    [System.Serializable]
    public class Shake : PlayableAsset, ITimelineClipAsset
    {
        //カメラを揺らす強さ
        [Header("カメラを揺らす強さ")]
        [SerializeField]
        private float magnitude = 0.2f;

        //Timelineで再生されるPlayableを生成する
        public override Playable CreatePlayable(
            PlayableGraph graph,
            GameObject owner)
        {
            //ShakePlayableを生成
            var playable = ScriptPlayable<ShakePlayable>.Create(graph);

            //Inspectorで設定した揺れの強さを渡す
            playable.GetBehaviour().magnitude = magnitude;

            return playable;
        }

        //クリップの長さ以外の編集機能は使用しない
        public ClipCaps clipCaps => ClipCaps.None;
    }
}
