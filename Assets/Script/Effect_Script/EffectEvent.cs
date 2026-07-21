using UnityEngine;
using Effekseer;

public class EffectEvent : MonoBehaviour
{
    [Header("再生するエフェクト")]
    [SerializeField]
    private EffekseerEmitter effect;

    public void PlayEffect()
    {
        if (effect != null)
        {
            effect.Play();
        }
    }
}
