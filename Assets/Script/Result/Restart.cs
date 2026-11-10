using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{

    // フェードアウト用Timeline
    [SerializeField]
    private PlayableDirector fadeOutDirector;

    // 二重実行防止
    private bool isStarting = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
	{
        // Timeline終了時に呼ばれるイベントを登録
        fadeOutDirector.stopped += OnFadeFinished;
    }

    private void OnDestroy()
    {
        fadeOutDirector.stopped -= OnFadeFinished;
    }

    // Update is called once per frame
    void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) && !isStarting)
        {
			// マウスカーソルの固定を解除
			Cursor.lockState = CursorLockMode.Confined;

            // マウスカーソルを表示
            Cursor.visible = true;

            isStarting = true;

            // フェードアウト開始
            fadeOutDirector.Play();
        }
	}

    // Timeline終了後に呼ばれる
    private void OnFadeFinished(PlayableDirector director)
    {
        SceneManager.LoadScene("TitleScene");
    }

}
