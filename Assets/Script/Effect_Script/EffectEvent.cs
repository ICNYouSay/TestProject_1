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

    [Header("5個目に再生するエフェクト")]
    [SerializeField]
    private EffekseerEmitter effect_5;

    [Header("6個目に再生するエフェクト")]
    [SerializeField]
    private EffekseerEmitter effect_6;

    [Header("7個目に再生するエフェクト")]
    [SerializeField]
    private EffekseerEmitter effect_7;

    [Header("8個目に再生するエフェクト")]
    [SerializeField]
    private EffekseerEmitter effect_8;

    [Header("9個目に再生するエフェクト")]
    [SerializeField]
    private EffekseerEmitter effect_9;

    [Header("10個目に再生するエフェクト")]
    [SerializeField]
    private EffekseerEmitter effect_10;

    [Header("再生するエフェクト")]
    [SerializeField]
    private EffekseerEmitter[] effect;

    //1個目に設定されたエフェクトを再生
    public void PlayEffect_1()
    {
        //if (effect_1 != null)
        //{
        //    effect_1.Play();
        //}
        if (effect.Length > 0 && effect[0] != null)
        {
            effect[0].Play();
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

    //5個目に設定されたエフェクトを再生
    public void PlayEffect_5()
    {
        if (effect_5 != null)
        {
            effect_5.Play();
        }
    }

    //6個目に設定されたエフェクトを再生
    public void PlayEffect_6()
    {
        if (effect_6 != null)
        {
            effect_6.Play();
        }
    }

    //7個目に設定されたエフェクトを再生
    public void PlayEffect_7()
    {
        if (effect_7 != null)
        {
            effect_7.Play();
        }
    }


    //8個目に設定されたエフェクトを再生
    public void PlayEffect_8()
    {
        if (effect_8 != null)
        {
            effect_8.Play();
        }
    }

    //9個目に設定されたエフェクトを再生
    public void PlayEffect_9()
    {
        if (effect_9 != null)
        {
            effect_9.Play();
        }
    }

    //10個目に設定されたエフェクトを再生
    public void PlayEffect_10()
    {
        if (effect_10 != null)
        {
            effect_10.Play();
        }
    }
}
