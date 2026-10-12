using UnityEngine;

public class CharacterSelectManager : MonoBehaviour
{
    public GameObject[] characterPrefabs; // プレハブを入れる箱
    public Transform spawnPoint;          // 出現場所を入れる箱

    private GameObject currentModel;      // 今出ているキャラを入れる箱

    public void SelectCharacter(int index)
    {
        // もし今キャラがいたら消す
        if (currentModel != null) Destroy(currentModel);

        // 新しいキャラを生成する
        currentModel = Instantiate(characterPrefabs[index], spawnPoint.position, spawnPoint.rotation, spawnPoint);

        // さらに生成されたキャラの回転をリセットする
        currentModel.transform.localPosition = Vector3.zero;        // 原点に配置
        currentModel.transform.localRotation = Quaternion.identity; // 角度をリセット

        // --- ここで攻撃用スクリプトを止める ---
        var controller = currentModel.GetComponent<PlayerController>();
        if (controller != null) controller.enabled = false;
        // ---------------------------------

        // ここで保存
        //GameData.SelectedCharacterIndex = index;

        Debug.Log("キャラを召喚しました: " + index);
    }
}
