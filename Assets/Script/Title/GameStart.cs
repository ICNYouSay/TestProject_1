using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            OnClickStartButton();
        }
    }

    // 何かしらのキーが押されたらゲームを開始する
    public void OnClickStartButton()
    {
        // ゲーム開始処理をここに書く
        Debug.Log("ゲーム開始");
        SceneManager.LoadScene("CharacterSelectScene");
    }
}
