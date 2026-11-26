using Fusion;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Rapca_Animation : CharacterAnimation
{
    private CharacterController controller;
    private Animator anim;
    private NetworkCharacterController _ncc;

    void Start()
    {
        controller = GetComponentInParent<CharacterController>();
        anim = GetComponent<Animator>();
        _ncc = GetComponentInParent<NetworkCharacterController>();
    }

    private void Update()
    {
    }

    public void NormalAttacked()
    {
        anim.SetBool("CanWalk", true);
    }

}