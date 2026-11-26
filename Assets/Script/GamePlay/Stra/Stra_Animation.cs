using Fusion;
using UnityEngine;

public class Stra_Animation : CharacterAnimation
{
    private Animator anim;
    private NetworkCharacterController _ncc;

    void Start()
    {
        anim = GetComponent<Animator>();
        _ncc = GetComponentInParent<NetworkCharacterController>();
    }

    public void NormalAttacked()
    {
        if (anim != null) anim.SetBool("CanWalk", true);
    }
}