using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using UnityEngine.Playables;
using UnityEngine.Timeline;

//TimeLine上でCameraの位置を制御するためのマーカー
[TrackBindingType(typeof(Animator))]
public class CameraControlMarker : Marker, INotification
{
    //CameraControlMarkerのIDを取得するためのプロパティ
    public PropertyName id => new();

    //CameraControlMarkerのLookAtを有効にするかどうかを設定するための変数
    [Header("LookAtを有効にするかどうか")]
    public bool isEnable = true;

    //CameraControlMarkerの向きをどう直すかを設定するための変数
    [Header("カメラの向きをどう直すか")]
    public bool isReturnRotation = true;

    //CameraControlMarkerの向きを戻す時間を設定するための変数
    [Header("カメラの向きを戻す時間")]
    public float returnDuration = 1.0f;
}
