using Fusion;
using UnityEngine;

public class Michael_Animation : CharacterAnimation
{
    private CharacterController controller;
    private Animator anim;
    private NetworkCharacterController _ncc;

    // キー入力判定用フラグ
    public bool _StraightPressed;
    public bool _KOStarPunchPressed; 

    void Start()
    {
        controller = GetComponentInParent<CharacterController>();
        anim = GetComponent<Animator>();
        _ncc = GetComponentInParent<NetworkCharacterController>();
    }

    private void Update()
    {
    }

    public void StraightPressed()
    {
        Debug.Log("G.O.ストレート発動！");
        _StraightPressed = true;
    }

    public void KOStarPunchPressed()
    {
        Debug.Log("K.O.スターパンチ発動！");
        _KOStarPunchPressed = true;

        // Animator側のトリガーを引く
        if (anim != null)
        {
            anim.SetTrigger("KOStarPunch"); 
        }
    }
}