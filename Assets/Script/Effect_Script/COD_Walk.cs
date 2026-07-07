using Effekseer;
using UnityEngine;

public class COD_Walk : MonoBehaviour
{

    // エフェクトを再生するための EffekseerEmitter コンポーネントの参照
    [Header("土煙エフェクト設定")]
    [SerializeField]
    private EffekseerEmitter dust;

    // エフェクトの位置を調整するためのオフセット値
    [Header("エフェクト位置調整")]
    [SerializeField]
    private float backOffset = 0.25f;

    //エフェクトを地面から少し上にずらすためのオフセット値
    [Header("地面から浮かせる高さ")]
    [SerializeField]
    private float heightOffset = 0.1f;

    // 入力があるかどうかを判定するためのフラグ
    private bool isPlaying;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //WASDキー（または矢印キー）の入力を -1.0 ～ 1.0 の間で取得
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // 入力があるかどうかを判定
        bool isMoving = moveX != 0 || moveZ != 0;

        //エフェクトの位置をプレイヤーに合わせる
        dust.transform.position =
            transform.position -
            transform.forward * backOffset
            + Vector3.up * heightOffset; // 少し上にずらす

        //エフェクトの向きをプレイヤーに合わせる
        dust.transform.rotation = transform.rotation;

        // 入力がある場合はエフェクトを再生し、入力がない場合は停止する
        if (isMoving)
        {
            if (!isPlaying)// 入力がある場合はエフェクトを再生
            {
                dust.Play();// エフェクトを再生
                isPlaying = true;// フラグを更新
            }
        }
        else
        {
            if (isPlaying)// 入力がない場合はエフェクトを停止
            {
                dust.Stop();// エフェクトを停止
                isPlaying = false;// フラグを更新
            }
        }
    }

}
