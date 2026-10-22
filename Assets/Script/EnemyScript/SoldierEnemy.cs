using UnityEngine;

//雑魚敵
public class SoldierEnemy : EnemyBase
{
    //プレイヤーを見つけたかどうか
    [Header("プレイヤーを見つけたかどうか")]
    [SerializeField]
    private bool canSeePlayer= true;

    //攻撃が終了したかどうか
    [Header("攻撃が終了したかどうか")]
    [SerializeField]
    private bool attackFinished = true;

    //------------------------
    // 初期化
    //------------------------
    protected override void Awake()
    {
        base.Awake();

        // 雑魚敵専用Transition
        transition = new SoldierTransition();
    }

    //------------------------
    //更新
    //------------------------
    protected override void Update()
    {
        base.Update();
    }

    //------------------------
    // プレイヤーを見つけたかどうかを返す
    //------------------------
    public bool CanSeePlayer()
    {
        return canSeePlayer;
    }

    //------------------------
    // 攻撃
    //------------------------
    public override void Attack()
    {
        Debug.Log("攻撃開始");

        attackFinished = false;
    }
}
