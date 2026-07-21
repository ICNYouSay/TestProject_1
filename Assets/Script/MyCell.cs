using FancyScrollView;
using UnityEngine;
using UnityEngine.UI;

public class MyCell : FancyCell<MenuCellData>
{
    [SerializeField] private Text nameText;
    [SerializeField] private Image iconImage;

    // ① データの更新（リストが動いて中身が変わった時）
    public override void UpdateContent(MenuCellData itemData)
    {
        nameText.text = itemData.Name;
        iconImage.sprite = itemData.Icon;
    }

    // ② 【最重要】位置の更新（スクロール中に毎フレーム呼ばれる）
    // position : 画面上での位置（画面中央付近が 0.5、上が 0.0、下が 1.0 になる）
    public override void UpdatePosition(float position)
    {
        // --- 1. 上下移動（Y軸） ---
        // 画面中央（0.5）からの距離を計算
        float distanceFromCenter = position - 0.5f;
        float yPos = distanceFromCenter * -900f; // 縦の広がり具合

        // --- 2. 斜め（ドラム型）にするための横移動（X軸） ---
        // Yの位置に応じて、右上に傾くようにX座標をオフセットする！
        float xPos = yPos * 0.8f + 500f;

        transform.localPosition = new Vector3(xPos, yPos, 0);

        // --- 3. 中央（選択中）のアイテムだけ大きく強調する ---
        float distanceAbs = Mathf.Abs(distanceFromCenter);
        float scale = Mathf.Lerp(0.12f, 0.07f, distanceAbs * 2f); // 中央は1.2倍、端は0.7倍
        transform.localScale = new Vector3(scale * 2, scale, 1f);

        // --- 4. 奥に引っ込んでいる感を出すために端を半透明にする ---
        float alpha = Mathf.Lerp(1.0f, 0.3f, distanceAbs * 2f);
        GetComponent<CanvasGroup>().alpha = alpha;
    }
}