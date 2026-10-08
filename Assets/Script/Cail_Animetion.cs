using Fusion;
using UnityEngine;

public class Cail_Animetion : MonoBehaviour
{
    private CharacterController controller;
    private Animator anim;
    private NetworkCharacterController _ncc;


    // キー入力判定用フラグ
    public bool _GungnirPressed;
    void Start()
    {
        controller = GetComponentInParent<CharacterController>();
        anim = GetComponent<Animator>();
        _ncc = GetComponentInParent<NetworkCharacterController>();
    }

    private void Update()
    {
    }

    public void GungnirPressed()
    {
        if (anim.GetInteger("Gungnir") == 0)
        {
            anim.SetInteger("Gungnir", 1);
            Debug.Log("Gungnir=1");
            _GungnirPressed = true;
        }
    }

    public void GungnirJumped()
    {
        if (anim.GetInteger("Gungnir") == 1)
        {
            anim.SetInteger("Gungnir", 2);
            Debug.Log("Gungnir=2");
            _ncc.Move(new Vector3(0.0f, 0.0f, 5.0f));
        }
    }

    public void GungnirAttacked()
    {
        if (anim.GetInteger("Gungnir") == 2)
        {
            anim.SetInteger("Gungnir", 0);
            Debug.Log("Gungnir=0");
            _GungnirPressed = false;
        }
    }
}