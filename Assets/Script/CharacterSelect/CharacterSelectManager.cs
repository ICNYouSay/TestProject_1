using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{
	[Header("全キャラクターのデータ(ScriptableObject)")]
	public CharacterData[] allCharacterData;

	[Header("3Dモデル生成場所")]
	public Transform spawnPoint;

	[Header("UI設定")]
	public GameObject okButton;             // OKボタン
	public GameObject characterChangeButton;// キャラチェンボタン
	public GameObject abilityChangeButton;  // アビチェンボタン
	public GameObject statusPanel;          // ステータスパネル
    public GameObject abilityPanel;          // アビリティパネル
    public GameObject characterList;          // キャラリスト
	public GameObject abilityList;          // アビリスト
	public TextMeshProUGUI nameText;        // 名前テキスト
	public TextMeshProUGUI statusText;      // ステータス表示テキスト
	public TextMeshProUGUI descriptionText; // キャラ説明テキスト
	private GameObject currentModel;

    [Header("メインカメラ")]
	public Camera mainCamera;

    [Header("ステータスゲージ")]
	public Slider hpSlider;
	public Slider atkSlider;
	public Slider defSlider;

	// キャラ選択、アビリティ選択それぞれのメニューを開いているかどうかを管理するフラグ
	enum MenuState
	{
		None = 0,
		CharacterSelect,
		AbilitySelect
	}
	private MenuState currentMenu = MenuState.CharacterSelect; // 初期値はキャラ選択メニュー

	private int EquipAbilityIndex = 0; // 選択中のアビリティスロットを管理する変数

    void Start()
	{
		ClearUI();
		SelectCharacter(0);
	}

	private void Update()
	{
		// 開いているメニューの状態に合わせて各種UIの位置を調整する
		if (currentMenu == MenuState.CharacterSelect)
		{
			// キャラ選択メニューを開いている場合のUI位置調整
			if (okButton != null) 
				okButton.transform.localPosition                += (new Vector3(-490, -350, 0) - okButton.transform.localPosition) / 30; ///< OKボタン
			if (characterChangeButton != null) 
				characterChangeButton.transform.localPosition   += (new Vector3(1300, 260, 0) - characterChangeButton.transform.localPosition) / 30; ///< キャラチェンボタン
			if (abilityChangeButton != null) 
				abilityChangeButton.transform.localPosition     += (new Vector3(1300, -100, 0) - abilityChangeButton.transform.localPosition) / 30; ///< アビチェンボタン
			if (statusPanel != null) 
				statusPanel.transform.localPosition             += (new Vector3(-440, 360, 0) - statusPanel.transform.localPosition) / 30; ///< ステータスパネル
			if (characterList != null) 
				characterList.transform.localPosition           += (new Vector3(728, 0, 0) - characterList.transform.localPosition) / 30; ///< キャラリスト
			if (abilityList != null) 
				abilityList.transform.localPosition             += (new Vector3(-1928, 0, 0) - abilityList.transform.localPosition) / 30; ///< アビリスト

            mainCamera.transform.localRotation = Quaternion.Lerp(mainCamera.transform.localRotation, Quaternion.Euler(0, 30, 0), Time.deltaTime * 2.0f);
        }
		else if (currentMenu == MenuState.AbilitySelect)
		{
			// アビリティ選択メニューを開いている場合のUI位置調整
			if (okButton != null) 
				okButton.transform.localPosition                += (new Vector3(490, -350, 0) - okButton.transform.localPosition) / 30; ///< OKボタン
			if (characterChangeButton != null)
				characterChangeButton.transform.localPosition   += (new Vector3(1300, 260, 0) - characterChangeButton.transform.localPosition) / 30; ///< キャラチェンボタン
			if (abilityChangeButton != null)
				abilityChangeButton.transform.localPosition     += (new Vector3(1300, -100, 0) - abilityChangeButton.transform.localPosition) / 30; ///< アビチェンボタン
			if (statusPanel != null) 
				statusPanel.transform.localPosition             += (new Vector3(-440, 720, 0) - statusPanel.transform.localPosition) / 30; ///< ステータスパネル
			if (characterList != null)
				characterList.transform.localPosition           += (new Vector3(1928, 0, 0) - characterList.transform.localPosition) / 30; ///< キャラリスト
			if (abilityList != null)
				abilityList.transform.localPosition             += (new Vector3(-728, 0, 0) - abilityList.transform.localPosition) / 30; ///< アビリスト

            mainCamera.transform.localRotation = Quaternion.Lerp(mainCamera.transform.localRotation, Quaternion.Euler(0, -30, 0), Time.deltaTime * 2.0f);
        }
		else if (currentMenu == MenuState.None)
		{
			// 何もメニューを開いていない場合のUI位置調整
			if (okButton != null) 
				okButton.transform.localPosition				+= (new Vector3(0, -300, 0) - okButton.transform.localPosition) / 30; ///< OKボタン
			if (characterChangeButton != null)
				characterChangeButton.transform.localPosition	+= (new Vector3(600, 260, 0) - characterChangeButton.transform.localPosition) / 30; ///< キャラチェンボタン
			if (abilityChangeButton != null)
				abilityChangeButton.transform.localPosition		+= (new Vector3(600, -100, 0) - abilityChangeButton.transform.localPosition) / 30; ///< アビチェンボタン
			if (statusPanel != null) 
				statusPanel.transform.localPosition				+= (new Vector3(-440, 720, 0) - statusPanel.transform.localPosition) / 30; ///< ステータスパネル
			if (characterList != null)
				characterList.transform.localPosition			+= (new Vector3(1928, 0, 0) - characterList.transform.localPosition) / 30; ///< キャラリスト
			if (abilityList != null)
				abilityList.transform.localPosition				+= (new Vector3(-1928, 0, 0) - abilityList.transform.localPosition) / 30; ///< アビリスト

			mainCamera.transform.localRotation = Quaternion.Lerp(mainCamera.transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 2.0f);
		}
	}

	void ClearUI()
	{
		if (nameText != null) nameText.text = "";
		if (statusText != null) statusText.text = "";
		if (descriptionText != null) descriptionText.text = "";
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

		// ステータスパネルを表示
		if (statusPanel != null) statusPanel.SetActive(true);

		// UIに名前とステータスを表示する！
		if (nameText != null) nameText.text = data.characterName;
		if (statusText != null)
		{
			statusText.text = $"HP　 : {data.hp:F2}\nATK  : {data.atk:F2}\nDEF  : {data.def:F2}";
		}
		if (descriptionText != null)
		{
			descriptionText.text = data.description;
		}

		// スライダーゲージの長さを反映
		if (hpSlider != null) hpSlider.value = data.hp;
		if (atkSlider != null) atkSlider.value = data.atk;
		if (defSlider != null) defSlider.value = data.def;

		characterChangeButton.GetComponentInChildren<TextMeshProUGUI>().text = data.characterName;
    }

	public void SelectAbility(int index)
	{
		// 選択中のアビリティスロットに装備するアビリティを設定する
		GameData.EquippedAbilities[EquipAbilityIndex] = index;


    }

    public void OnClickOKButton()
	{
		if (currentMenu == MenuState.CharacterSelect)
		{
			// キャラ選択メニューを閉じる
			currentMenu = MenuState.None;
			okButton.GetComponentInChildren<TextMeshProUGUI>().text = "GO!MOOOOOVE!!";
		}
		else if (currentMenu == MenuState.AbilitySelect)
		{
			// アビリティ選択メニューを閉じる
			currentMenu = MenuState.None;
			okButton.GetComponentInChildren<TextMeshProUGUI>().text = "GO!MOOOOOVE!!";
		}
		else if (currentMenu == MenuState.None)
		{
			// 何もメニューを開いていない状態(ボタンのテキストが出撃になっている状態)ならゲームシーンに遷移する
			SceneManager.LoadScene("GamePlayScene");
		}
	}


	public void OnClickCharacterChangeButton()
	{
		currentMenu = MenuState.CharacterSelect;
		okButton.GetComponentInChildren<TextMeshProUGUI>().text = "OK";
	}

	public void OnClickAbilityChangeButton(int index)
	{
		currentMenu = MenuState.AbilitySelect;
		okButton.GetComponentInChildren<TextMeshProUGUI>().text = "OK";
		EquipAbilityIndex = index;
    }
}