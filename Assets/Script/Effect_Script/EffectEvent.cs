using UnityEngine;
using Effekseer;

public class EffectEvent : MonoBehaviour
{
    [Header("1個目に再生するエフェクト")]
    [SerializeField]
    private EffekseerEmitter effect_1;

    [Header("2個目に再生するエフェクト")]
    [SerializeField]
    private EffekseerEmitter effect_2;

    [Header("3個目に再生するエフェクト")]
    [SerializeField]
    private EffekseerEmitter effect_3;

    [Header("4個目に再生するエフェクト")]
    [SerializeField]
    private EffekseerEmitter effect_4;

    //1個目に設定されたエフェクトを再生
    public void PlayEffect_1()
    {
        if (effect_1 != null)
        {
            effect_1.Play();
        }
    }

    //2個目に設定されたエフェクトを再生
    public void PlayEffect_2()
    {
        if (effect_2 != null)
        {
            effect_2.Play();
        }
    }

    //3個目に設定されたエフェクトを再生
    public void PlayEffect_3()
    {
        if (effect_3 != null)
        {
            effect_3.Play();
        }
    }

    //4個目に設定されたエフェクトを再生
    public void PlayEffect_4()
    {
        if (effect_4 != null)
        {
            effect_4.Play();
        }
    }

}
