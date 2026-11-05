using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;


//Markerからの通知を受け取るためのクラス
[RequireComponent(typeof(Animator))]
public sealed class CameraControlMarkerReceiver : MonoBehaviour, INotificationReceiver
{
    //カメラのLookAtターゲットとして設定するTransformオブジェクト
    [SerializeField]
    private Transform headTarget;

    //メインカメラのCinemachineBrainコンポーネントの参照
    private CinemachineBrain _cinemachineBrain;

    //カメラの回転補間を開始する前の状態を保持する変数
    private Quaternion _prevRotation;

    //回転補間処理中であることを示すフラグ
    private bool _isReturningRotation;

    //回転補間の進行状況を保持するタイマー
    private float _rotationLerpTime;

    //現在処理中のCameraConrtolMarkerの参照
    private CameraControlMarker _cameraControlMarker;

    private void Awake()
    {
        if (Camera.main != null)
        {
            //初期化時にCinemachineBrainを取得
            //MainCameraにアタッチされたCinemachineを取得
            _cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        }
    }


    //TimeLine上の通知を受け取る処理
    public void OnNotify(Playable _orozin, INotification _notification, object _context)
    {
        //通知がCinemaControlMaker形であるかをチェックし、キャスト
        _cameraControlMarker = _notification as CameraControlMarker;
        if (_cameraControlMarker == null)
        {
            return;
        }

        //Markerの設定に基づいてLookAt開始または停止を切り替える
        if (_cameraControlMarker.isEnable)
        {
            StartLookAt();
        }
        else
        {
            StopLookAt();
        } 
    }


    //LookAtを開始する処理
    private void StartLookAt()
    {
        //現在アクティブなCinemachineカメラを取得
        var activeCamera = _cinemachineBrain.ActiveVirtualCamera as CinemachineVirtualCamera;
        if (activeCamera ==null)
        {
            return;
        }

        //現在のカメラの回転を記録し、LookAtターゲットを設定
        _prevRotation = activeCamera.transform.rotation;
        activeCamera.LookAt = headTarget;
        _isReturningRotation = false;

    }

    //LookAtを停止する処理
    private void StopLookAt()
    {
        //現在アクティブなCinemachineカメラを取得
        var activeCamera = _cinemachineBrain.ActiveVirtualCamera as CinemachineVirtualCamera;
        if (activeCamera == null || _cameraControlMarker.isReturnRotation == false)
        {
            return;
        }

        //LookAtを解除してカメラの注視を停止
        activeCamera.LookAt = null;

        //補間用のフラグとタイマーをリセット
        //回転補間処理を開始
        _isReturningRotation = true;
        _rotationLerpTime = 0f;
    }


    //フレームごとに回転補間処理を実行する
    private void Update()
    {
        //回転補間が有効でなければスキップ
        if (!_isReturningRotation)
        {
            return;
        }

        //現在アクティブなCinemachineカメラを取得
        var activeCamera=_cinemachineBrain.ActiveVirtualCamera as CinemachineVirtualCamera;
        if (activeCamera == null)
        {
            return;
        }

        //時間に基づく回転補間を計算(returnDurationに基づく補間速度)
        _rotationLerpTime += Time.deltaTime / _cameraControlMarker.returnDuration;

        //Quternion.Slerpを使用して「滑らかに元の回転に戻す
        activeCamera.transform.rotation = Quaternion.Slerp(activeCamera.transform.rotation, _prevRotation, _rotationLerpTime);

        //補完が完了したら終了
        if (_rotationLerpTime >= 1.0f)
        {
            _isReturningRotation = false;
        }
    }
}
