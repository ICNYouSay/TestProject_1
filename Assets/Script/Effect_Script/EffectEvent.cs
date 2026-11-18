using UnityEngine;
using Effekseer;

public class EffectEvent : MonoBehaviour
{
    [Header("再生するエフェクト")]
    [SerializeField]
    private EffekseerEmitter[] effect;

    //1個目に設定されたエフェクトを再生
    public void PlayEffect_1()
    {
        if (effect.Length > 0 && effect[0] != null)
        {
            effect[0].Play();
        }
    }

    //2個目に設定されたエフェクトを再生
    public void PlayEffect_2()
    {
        if (effect.Length > 0 && effect[1] != null)
        {
            effect[1].Play();
        }
    }

    //3個目に設定されたエフェクトを再生
    public void PlayEffect_3()
    {
        if (effect.Length > 0 && effect[2] != null)
        {
            effect[2].Play();
        }
    }

    //4個目に設定されたエフェクトを再生
    public void PlayEffect_4()
    {
        if (effect.Length > 0 && effect[3] != null)
        {
            effect[3].Play();
        }
    }

    //5個目に設定されたエフェクトを再生
    public void PlayEffect_5()
    {
        if (effect.Length > 0 && effect[4] != null)
        {
            effect[4].Play();
        }
    }

    //6個目に設定されたエフェクトを再生
    public void PlayEffect_6()
    {
        if (effect.Length > 0 && effect[5] != null)
        {
            effect[5].Play();
        }
    }

    //7個目に設定されたエフェクトを再生
    public void PlayEffect_7()
    {
        if (effect.Length > 0 && effect[6] != null)
        {
            effect[6].Play();
        }
    }


    //8個目に設定されたエフェクトを再生
    public void PlayEffect_8()
    {
        if (effect.Length > 0 && effect[7] != null)
        {
            effect[7].Play();
        }
    }

    //9個目に設定されたエフェクトを再生
    public void PlayEffect_9()
    {
        if (effect.Length > 0 && effect[8] != null)
        {
            effect[8].Play();
        }
    }

    //10個目に設定されたエフェクトを再生
    public void PlayEffect_10()
    {
        if (effect.Length > 0 && effect[9] != null)
        {
            effect[9].Play();
        }
    }
}
