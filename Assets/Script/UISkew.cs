using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Effects/UI Skew")]
public class UISkew : BaseMeshEffect
{
    [SerializeField, Range(-100f, 100f)]
    private float skewX = -20f; // 横方向の傾き具合

    public float SkewX
    {
        get => skewX;
        set { skewX = value; if (graphic != null) graphic.SetVerticesDirty(); }
    }

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive()) return;

        UIVertex vertex = new UIVertex();
        for (int i = 0; i < vh.currentVertCount; i++)
        {
            vh.PopulateUIVertex(ref vertex, i);

            // Y座標（高さ）に応じてX位置をずらすことで、きれいな平行四辺形にする
            vertex.position.x += vertex.position.y * (skewX / 100f);

            vh.SetUIVertex(vertex, i);
        }
    }
}