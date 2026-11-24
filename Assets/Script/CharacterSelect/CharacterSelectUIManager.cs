using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectUIManager : MonoBehaviour
{
	[Header("全キャラクターのデータ(ScriptableObject)")]
	public CharacterData[] allCharacterData;

    [Header("全アビリティのデータ(ScriptableObject)")]
    public CharacterData[] allAbilityData;

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

    [Header("アビリティ用のパーツ")]
    public Slider abi_hpSlider;
    public Slider abi_atkSlider;
    public Slider abi_defSlider;
    public TextMeshProUGUI abi_nameText;        // 名前テキスト
    public TextMeshProUGUI abi_statusText;      // ステータス表示テキスト

	[Header("フェードアウト用Timeline")]
    [SerializeField]
    private PlayableDirector fadeOutDirector;

    // 二重実行防止
    private bool isStarting = false;

	[Header("フェード用Image")]
	[SerializeField]
	public Image fadeImage; // フェード用のImageコンポーネント


	[Header("カーソル")]
	public GameObject cursorImage; // カーソル用のImageコンポーネント

    private Gamepad gamepad;

	private int cursorPos = (int)MenuItem.OKButton;
    // メニュー項目の一覧を整理する列挙型
    enum MenuItem
	{
		None = 0,

		OKButton,
        CharacterSelect,
        AbilitySelect1,
		AbilitySelect2,
		AbilitySelect3,
		AbilitySelect4,
		AbilitySelect5,

		Character1,
		Character2,
		Character3,
		Character4,

		Ability1,
		Ability2,
		Ability3,
		Ability4,
		Ability5,
		Ability6,
		Ability7,
		Ability8,
		Ability9,
		Ability10,
    }

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

		fadeImage.transform.SetAsFirstSibling(); // フェード用Imageを最前面に移動

        // Timeline終了時に呼ばれるイベントを登録
        fadeOutDirector.stopped += OnFadeFinished;
    }

    private void OnDestroy()
    {
        fadeOutDirector.stopped -= OnFadeFinished;
    }

    private void Update()
	{
		gamepad = Gamepad.current;

        SetUIPosition();
		OnControllerInput();

		UpdateCursorPos();
    }

	// このシーンをコントローラーで操作するための関数
	public void OnControllerInput()
	{
		if (gamepad == null) return;

        Vector2 leftStick = gamepad.leftStick.ReadValue();
        if (currentMenu == MenuState.CharacterSelect)
		{
			int cursorInMenuPos = cursorPos - (int)MenuItem.Character1;
            // キャラ選択メニューが開かれてるとき
            // 十字キー及び←スティックの上下入力でキャラ選択
            if (input.y > 0.5f || (gamepad != null && gamepad.dpad.up.wasPressedThisFrame))
            {
                GameData.SelectedCharacterIndex = (GameData.SelectedCharacterIndex + 1) % allCharacterData.Length;
			}
            else if (input.y < -0.5f || (gamepad != null && gamepad.dpad.down.wasPressedThisFrame))
            {
                GameData.SelectedCharacterIndex = (GameData.SelectedCharacterIndex - 1 + allCharacterData.Length) % allCharacterData.Length;
			}
		}
		else if (currentMenu == MenuState.AbilitySelect)
        {
            int cursorInMenuPos = cursorPos - (int)MenuItem.Ability1;
            // アビリティ選択メニューが開かれてるとき
            if (leftStick.x > 0.5f || gamepad.dpad.right.wasPressedThisFrame)
			{
                cursorPos = ((cursorPos + 1) % allAbilityData.Length) + (int)MenuItem.Ability1;
			}
			else if (leftStick.x < -0.5f || gamepad.dpad.left.wasPressedThisFrame)
			{
                cursorPos = ((cursorPos - 1 + allAbilityData.Length) % allAbilityData.Length) + (int)MenuItem.Ability1;
			}
        }
        else if (currentMenu == MenuState.None)
		{
            // 何も開かれていないとき
            if (leftStick.x > 0.5f || gamepad.dpad.right.wasPressedThisFrame)
            {
                cursorPos = ((cursorPos + 1) % allAbilityData.Length) + (int)MenuItem.Ability1;
            }
            else if (leftStick.x < -0.5f || gamepad.dpad.left.wasPressedThisFrame)
            {
                cursorPos = ((cursorPos - 1 + allAbilityData.Length) % allAbilityData.Length) + (int)MenuItem.Ability1;
            }
        }

		// 決定ボタンを押し込め
		if (gamepad.buttonEast.wasPressedThisFrame)
		{
            switch ((MenuItem)cursorPos)
            {
                case MenuItem.OKButton:
					OnClickOKButton();
                    break;
                case MenuItem.CharacterSelect:
					OnClickCharacterChangeButton();
                    break;
                case MenuItem.AbilitySelect1:
                case MenuItem.AbilitySelect2:
                case MenuItem.AbilitySelect3:
                case MenuItem.AbilitySelect4:
                case MenuItem.AbilitySelect5:
                    int abilityIndex = cursorPos - (int)MenuItem.AbilitySelect1;
					OnClickAbilityChangeButton(abilityIndex);
                    break;
                case MenuItem.Character1:
                case MenuItem.Character2:
                case MenuItem.Character3:
                case MenuItem.Character4:
                    int characterIndex = cursorPos - (int)MenuItem.Character1;
					SelectCharacter(characterIndex);
                    break;
                case MenuItem.Ability1:
                case MenuItem.Ability2:
                case MenuItem.Ability3:
                case MenuItem.Ability4:
                case MenuItem.Ability5:
                case MenuItem.Ability6:
                case MenuItem.Ability7:
                case MenuItem.Ability8:
                case MenuItem.Ability9:
                case MenuItem.Ability10:
                    int abilityListIndex = cursorPos - (int)MenuItem.Ability1;
					SelectAbility(abilityListIndex);
                    break;
            }
        }
    }
    
	void UpdateCursorPos()
	{
		if (gamepad == null)
		{
            cursorImage.transform.position = new Vector3(-1000, -1000, 0); // コントローラーが接続されていない場合はカーソルを画面外に移動
			return;
        }

		// カーソルの位置を更新する
		Vector3 targetPos = Vector3.zero;
		switch ((MenuItem)cursorPos)
		{
			case MenuItem.OKButton:
				targetPos = okButton.transform.position - new Vector3(100, 0, 0);
				break;
			case MenuItem.CharacterSelect:
				targetPos = characterChangeButton.transform.position - new Vector3(100, 0, 0);
				break;
			case MenuItem.AbilitySelect1:
			case MenuItem.AbilitySelect2:
			case MenuItem.AbilitySelect3:
			case MenuItem.AbilitySelect4:
			case MenuItem.AbilitySelect5:
				int abilityIndex = cursorPos - (int)MenuItem.AbilitySelect1;
				targetPos = abilityChangeButton.transform.position + new Vector3(abilityIndex * 100, 0, 0);
				break;
			case MenuItem.Character1:
			case MenuItem.Character2:
			case MenuItem.Character3:
			case MenuItem.Character4:
				int characterIndex = cursorPos - (int)MenuItem.Character1;
				targetPos = characterList.transform.position + new Vector3(characterIndex * 200, 0, 0);
				break;
			case MenuItem.Ability1:
			case MenuItem.Ability2:
			case MenuItem.Ability3:
			case MenuItem.Ability4:
			case MenuItem.Ability5:
			case MenuItem.Ability6:
			case MenuItem.Ability7:
			case MenuItem.Ability8:
			case MenuItem.Ability9:
			case MenuItem.Ability10:
				int abilityListIndex = cursorPos - (int)MenuItem.Ability1;
				targetPos = abilityList.transform.position + new Vector3(abilityListIndex * 100, 0, 0);
				break;
		}
		cursorImage.transform.position = Vector3.Lerp(cursorImage.transform.position, targetPos, Time.deltaTime * 10f);
    }

	// UIの配置を管理する関数
    void SetUIPosition()
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
				abilityChangeButton.transform.localPosition     += (new Vector3(600, 360, 0) - abilityChangeButton.transform.localPosition) / 30; ///< アビチェンボタン
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
			okButton.GetComponentInChildren<TextMeshProUGUI>().text = "出撃!!";
		}
		else if (currentMenu == MenuState.AbilitySelect)
		{
			// アビリティ選択メニューを閉じる
			currentMenu = MenuState.None;
			okButton.GetComponentInChildren<TextMeshProUGUI>().text = "出撃!!";
		}
		else if (currentMenu == MenuState.None && !isStarting)
		{

			isStarting = true;

			fadeImage.transform.SetAsLastSibling(); // フェード用Imageを最前面に移動

			// フェードアウト開始
			fadeOutDirector.Play();

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

    // Timeline終了後に呼ばれる
    private void OnFadeFinished(PlayableDirector director)
    {
        // 何もメニューを開いていない状態(ボタンのテキストが出撃になっている状態)ならゲームシーンに遷移する
        SceneManager.LoadScene("GamePlayScene");

    }
}