using UnityEngine;

public class GameData
{
    // 選ばれたキャラクターの番号
    public static int SelectedCharacterIndex = 0;

    public static int[] EquippedAbilities = new int[5] { -1, -1, -1, -1, -1 }; // 初期値は-1（未装備）
}
