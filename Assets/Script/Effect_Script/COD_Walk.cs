using Effekseer;
using UnityEngine;

public class COD_Walk : MonoBehaviour
{

    [Header("土煙プレハブ")]
    [SerializeField]
    private EffekseerEmitter dustPrefab;

    [Header("エフェクト位置調整")]
    [SerializeField]
    private float backOffset = 0.25f;

    [SerializeField]
    private float heightOffset = 0.1f;

    [Header("何m移動したら生成するか")]
    [SerializeField]
    private float spawnDistance = 0.5f;

    // 前回生成した位置
    private Vector3 lastSpawnPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastSpawnPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //WASDキー（または矢印キー）の入力を -1.0 ～ 1.0 の間で取得
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // 移動しているかどうかを判定
        bool isMoving = moveX != 0 || moveZ != 0;

        //動いていなければ何もしない
        if (!isMoving)
            return;

        // 一定距離移動したら生成
        if (Vector3.Distance(transform.position, lastSpawnPosition) >= spawnDistance)
        {
            // エフェクトの生成位置を計算
            Vector3 pos =
                transform.position
                - transform.forward * backOffset
                + Vector3.up * heightOffset;

            // エフェクトを生成して再生
            EffekseerEmitter effect = Instantiate(
                dustPrefab,
                pos,
                transform.rotation);

            //エフェクトを再生
            effect.Play();

            // エフェクトの寿命後に削除
            Destroy(effect.gameObject, 0.5f);

            lastSpawnPosition = transform.position;
        }
    }

}
