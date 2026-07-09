using UnityEngine.Timeline;
using UnityEngine.UI;

//TimelineでFadeトラックの制御を行うスクリプト

namespace Timeline
{
    [TrackClipType(typeof(Fadeout))]
    [TrackBindingType(typeof(Image))]

    //TrackAssetを継承したFadeTrackクラスを定義
    public class FadeTrack :TrackAsset
    {
    }
}
