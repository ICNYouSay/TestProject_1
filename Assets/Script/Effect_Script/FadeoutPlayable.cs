using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Timeline
{
    [System.Serializable]
    public class FadeoutPlayable : PlayableBehaviour
    {
        public enum FadeType
        {
            FadeIn,
            FadeOut
        }

        public FadeType fadeType;

        public override void ProcessFrame(
            Playable playable,
            FrameData info,
            object playerData)
        {
            Image image = playerData as Image;

            if (image == null)
                return;

            double time = playable.GetTime();
            double duration = playable.GetDuration();

            float t = duration > 0
                ? (float)(time / duration)
                : 0f;

            Color color = image.color;

            color.a = fadeType == FadeType.FadeOut
                ? t
                : 1f - t;

            image.color = color;
        }
    }
}