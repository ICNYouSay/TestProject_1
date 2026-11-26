using Fusion;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Rapca_Animation : CharacterAnimation
{
    private CharacterController controller;
    private Animator anim;
    private NetworkCharacterController _ncc;

    // キー入力判定用フラグ
    public bool _TimeMagicPressed;

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

    public void TimeMagicPressed()
    {
        Debug.Log("禁断の時魔法 発動！");
        _TimeMagicPressed = true;
    }

}