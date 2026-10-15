using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CharacterSelectManager : MonoBehaviour
{
    [Header("全キャラクターのデータ(ScriptableObject)")]
    public CharacterData[] allCharacterData;

    [Header("3Dモデル生成場所")]
    public Transform spawnPoint;

    [Header("UI設定")]
    public GameObject okButton;
    public TextMeshProUGUI nameText;        // 名前テキスト
    public TextMeshProUGUI statusText;      // ステータス表示テキスト

    private GameObject currentModel;

    void Start()
    {
        if (okButton != null) okButton.SetActive(false);
        ClearUI();
    }

    void ClearUI()
    {
        if (nameText != null) nameText.text = "";
        if (statusText != null) statusText.text = "";
    }

    public void SelectCharacter(int index)
    {
        if (allCharacterData == null || index >= allCharacterData.Length) return;

        // 保管庫に選んだ番号をセット
        GameData.SelectedCharacterIndex = index;

        // データからキャラ情報を取り出す
        CharacterData data = allCharacterData[index];

        // 3Dモデルの生成
        if (currentModel != null) Destroy(currentModel);
        currentModel = Instantiate(data.modelPrefab, spawnPoint.position, spawnPoint.rotation, spawnPoint);
        currentModel.transform.localPosition = Vector3.zero;

        // 設定した向きの補正
        currentModel.transform.localRotation = Quaternion.Euler(0, data.rotateoffset, 0);

        // キャラセレ画面で勝手に動かないように操作スクリプトをオフにする
        var controller = currentModel.GetComponent<PlayerController>();
        if (controller != null) controller.enabled = false;

        // OKボタンを表示
        if (okButton != null) okButton.SetActive(true);

        // UIに名前とステータスを表示する！
        if (nameText != null) nameText.text = data.characterName;
        if (statusText != null)
        {
            statusText.text = $"COST : {data.maxCost}\nATK  : {data.atk}\nDEF  : {data.def}";
        }
    }

    public void OnClickOKButton()
    {
        SceneManager.LoadScene("GamePlayScene");
    }
}