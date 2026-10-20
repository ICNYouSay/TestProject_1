using UnityEngine;

//G‹›“G
public class SoldierEnemy : EnemyBase
{
    //ƒvƒŒƒCƒ„[‚ğŒ©‚Â‚¯‚½‚©‚Ç‚¤‚©
    [SerializeField]
    private bool canSeePlayer= true;

    //UŒ‚‚ªI—¹‚µ‚½‚©‚Ç‚¤‚©
    [SerializeField]
    private bool attackFinished = true;

    //------------------------
    // ‰Šú‰»
    //------------------------
    protected override void Awake()
    {
        base.Awake();

        // G‹›“Gê—pTransition
        transition = new SoldierTransition();
    }

    //------------------------
    // ƒvƒŒƒCƒ„[‚ğŒ©‚Â‚¯‚½‚©‚Ç‚¤‚©‚ğ•Ô‚·
    //------------------------
    public bool CanSeePlayer()
    {
        return canSeePlayer;
    }

    //------------------------
    // UŒ‚
    //------------------------
    public override void Attack()
    {
        Debug.Log("UŒ‚ŠJn");

        attackFinished = false;
    }

    public override void Chase()
    {
    }

    public override void StopMove()
    {
        //ˆÚ“®’â~‚Ìˆ—
        Debug.Log("ˆÚ“®’â~");

        transform.Translate(Vector3.zero);
    }

}
