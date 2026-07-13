using UnityEngine;
using UnityEngine.Playables;

namespace Timeline
{
    //Timeline再生中にカメラを揺らす処理
    [System.Serializable]
    public class ShakePlayable :  PlayableBehaviour
    {
        //揺れの強さ
        [Header("揺れの強さ")]
        public float magnitude = 0.2f;

        //カメラの元の位置
        private Vector3 orizinalPosition;

        //初期位置を保存したかどうかのフラグ
        private bool initialized = false;

        //揺らす対象のカメラ
        private Camera targetCamera;

        //Timeline再生中、毎フレーム呼ばれる処理
        public override void ProcessFrame(
            Playable playable,
            FrameData info,
            object playerData)
        {
            //TrackにバインドしたCameraを取得
            Camera camera = playerData as Camera;

            //後で元の位置へ戻すために保存
            targetCamera = camera;

            //Cameraが設定されていない場合は処理しない
            if (camera == null)
                return;

            //最初の1回だけカメラの初期位置を保存
            if (!initialized)
            {
                orizinalPosition = camera.transform.localPosition;
                initialized = true;
            }

            //ランダムな揺れ量を生成
            Vector2 offset = Random.insideUnitCircle * magnitude;

            //カメラをランダムに揺らす
            camera.transform.localPosition =
                orizinalPosition +
                new Vector3(offset.x, offset.y, 0f);
        }

        //Timelineの再生が終了した時に呼ばれる
        public override void OnBehaviourPause(
            Playable playable,
            FrameData info)
        {
            //カメラを元の位置に戻す
            if (targetCamera != null)
            {
                targetCamera.transform.localPosition = orizinalPosition;
            }

            //次回再生のため初期化フラグをリセット
            initialized = false;
        }
    }
}
