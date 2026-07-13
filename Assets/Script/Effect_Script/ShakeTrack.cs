using UnityEngine.Timeline;
using UnityEngine;


//TimelineでShakeトラックの制御を行うスクリプト
namespace Timeline
{

    [TrackClipType(typeof(Shake))]
    [TrackBindingType(typeof(Camera))]

    //TrackAssetを継承したShakeTrackクラスを定義
    public class ShakeTrack : TrackAsset
    {
    }
}
