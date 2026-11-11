using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class GameStart : MonoBehaviour
{
    // フェードアウト用Timeline
    [SerializeField]
    private PlayableDirector fadeOutDirector;

    // 二重実行防止
    private bool isStarting = false;

    private void Start()
    {
        // Timeline終了時に呼ばれるイベントを登録
        fadeOutDirector.stopped += OnFadeFinished;
    }

    private void OnDestroy()
    {
        fadeOutDirector.stopped -= OnFadeFinished;
    }

    private void Update()
    {
        if (Input.anyKeyDown && !isStarting)
        {
            OnClickStartButton();
        }
    }

    // Startボタンが押されたときの処理
    public void OnClickStartButton()
    {
        isStarting = true;

        // フェードアウト開始
        fadeOutDirector.Play();
    }

    // Timeline終了後に呼ばれる
    private void OnFadeFinished(PlayableDirector director)
    {
        SceneManager.LoadScene("CharacterSelectScene");
    }
}