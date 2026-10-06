using System.Collections.Generic;
using UnityEngine;

public class MenuTest : MonoBehaviour
{
    [SerializeField] private MyScrollView myScrollView;

    private void Start()
    {
        var items = new List<MenuCellData>();
        for (int i = 0; i < 10; i++)
        {
            items.Add(new MenuCellData { Name = $"キャラ {i + 1}", ID = i });
        }

        // データを渡すと自動的にスクロールビューが生成される！
        myScrollView.UpdateData(items);
    }
}