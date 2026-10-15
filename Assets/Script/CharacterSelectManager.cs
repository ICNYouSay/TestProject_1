using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectManager : MonoBehaviour
{
    public GameObject[] characterPrefabs; // プレハブを入れる箱
    public Transform spawnPoint;          // 出現場所を入れる箱

    private GameObject currentModel;      // 今出ているキャラを入れる箱

    // OKボタンを登録する枠
    [Header("UI設定")]
    public GameObject okButton;

    // 現在選択中のインデックスを保持
    private int currentSelectedIndex = 0;

    void Start()
    {
        // ゲーム開始時はOKボタンを非表示にしておく
        if (okButton != null)
        {
            okButton.SetActive(false);
        }
    }

    public void SelectCharacter(int index)
    {
        // キャラのインデックスの譲渡
        GameData.SelectedCharacterIndex = index;

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

        // キャラが選ばれたのでOKボタンを表示する
        if (okButton != null)
        {
            okButton.SetActive(true);
        }

    }
    // OKボタンを押した時に呼ぶ関数
    public void OnClickOKButton()
    {
        // GamePlaySceneに切り替える
        SceneManager.LoadScene("GamePlayScene");
    }

}
