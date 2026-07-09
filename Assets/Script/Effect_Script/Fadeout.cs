using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline
{
    [System.Serializable]
    public class Fadeout : PlayableAsset, ITimelineClipAsset
    {
        [SerializeField]
        FadeoutPlayable.FadeType type = FadeoutPlayable.FadeType.FadeIn;

        public override Playable CreatePlayable(
            PlayableGraph graph,
            GameObject owner)
        {
            var playable = ScriptPlayable<FadeoutPlayable>.Create(graph);

            playable.GetBehaviour().fadeType = type;

            return playable;
        }

        public ClipCaps clipCaps => ClipCaps.None;
    }
}