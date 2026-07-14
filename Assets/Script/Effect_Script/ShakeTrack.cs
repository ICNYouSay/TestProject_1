using UnityEngine.Timeline;
using UnityEngine;
using Cinemachine;


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
