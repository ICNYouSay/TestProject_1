using System.Collections.Generic;
using FancyScrollView;
using UnityEngine;

public class MyScrollView : FancyScrollView<MenuCellData>
{
    [SerializeField] private Scroller scroller;
    [SerializeField] private GameObject cellPrefab;

    protected override GameObject CellPrefab => cellPrefab;

    private IList<MenuCellData> CharacterList;
    private int selectedIndex = 0;

    private void Start()
    {
        // スクロール位置が変わった時に見え方を更新するイベント登録
        scroller.OnValueChanged(UpdatePosition);
    }

    private void Update()
    {
        if (CharacterList != null)
        {
        }
    }

    // 外部からデータを流し込む関数
    public void UpdateData(IList<MenuCellData> items)
    {
        CharacterList = items;
    }
}