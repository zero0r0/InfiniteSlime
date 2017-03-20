using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    enum GameState {
        READY,
        PLAY,
        GAMEOVER,
        GAMECLEAR,
    }

    private GameState nowState;

    [SerializeField]
    private GameObject gameStart;

    [SerializeField]
    private Player player;
    [SerializeField]
    private Brave brave;
    [SerializeField]
    private Background[] background;

    [SerializeField]
    private StageManager stageManager;

    [SerializeField]
    private AudioClip selectSe;
    [SerializeField]
    private AudioClip gameOverBGM;

    private bool isMoveScene = false;

    private bool isHumanized = false;

    [SerializeField]
    private float particleTime = 1f;


    void Awake() {
        player.enabled = false;
        brave.enabled = false;
        background[0].enabled = false;
        background[1].enabled = false;
        isMoveScene = false;
    }

    // Use this for initialization
    void Start() {
        nowState = GameState.READY;
        gameStart.SetActive(true);
    }

    // Update is called once per frame
    void Update() {
        switch (nowState) {
            case GameState.READY:
                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
                    GameStart();

                break;

            case GameState.PLAY:
                if (player.Mind >= 100 && (Input.GetKeyDown(KeyCode.Space) || isHumanized)) {
                    GameClear();
                }

                break;

            case GameState.GAMEOVER:
                if (Input.GetMouseButtonUp(0) || (Input.GetKeyDown(KeyCode.Space))) {
                    SceneManager.LoadScene(0);
                }

                break;
            case GameState.GAMECLEAR:
                if (isMoveScene && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))) {
                    SceneManager.LoadScene(0);
                }
                break;

        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }

    }

    private void GameStart() {
        SystemSetBool(true);
        gameStart.SetActive(false);
        SoundManager.instance.IsPlaying = true;
        SoundManager.instance.SoundSystemSE(selectSe);
        SoundManager.instance.SoundMainGameBGM();
        nowState = GameState.PLAY;
    }

    public void GameOver() {
        SystemSetBool(false);
        nowState = GameState.GAMEOVER;
        SoundManager.instance.IsPlaying = false;
        SoundManager.instance.StopBGM();
        SoundManager.instance.SoundSystemSE(gameOverBGM);
        EndingManager.instance.JudgeEnding(true, 0, player.HP);
    }

    private void GameClear() {
        SystemSetBool(false);
        stageManager.IsPlaying = false;
        UIManager.instance.SaveHighScore();
        nowState = GameState.GAMECLEAR;
        StartCoroutine(ProductionGameClear());
        SoundManager.instance.IsPlaying = false;
        SoundManager.instance.StopBGM();

    }

    /// <summary>
    /// エンディングの演出
    /// </summary>
    /// <returns></returns>
    IEnumerator ProductionGameClear() {
        player.evolution.Play();
        yield return new WaitForSeconds(particleTime);
        yield return StartCoroutine(EndingManager.instance.FlashImage(Color.white, 0, 1));
        player.evolution.Stop();
        EndingManager.instance.JudgeEnding(false, player.Mind);
        UIManager.instance.SetResult();
        player.gameObject.SetActive(false);
        yield return StartCoroutine(EndingManager.instance.FlashImage(Color.white, 1, 0));
        isMoveScene = true;
    }

    /// <summary>
    /// メインゲームプレイ中でない場合（ゲームオーバーやクリアなどで）又はその逆
    /// 各オブジェクトの動作を止めたりスタートさせたりする関数
    /// </summary>
    /// <param name="isPlay"></param>
    private void SystemSetBool(bool isPlay) {
        player.enabled = isPlay;
        brave.enabled = isPlay;
        background[0].enabled = isPlay;
        background[1].enabled = isPlay;
        stageManager.IsPlaying = isPlay;
        UIManager.instance.IsPlaying = isPlay;
    }

    public void SetHumanized(bool setHumanized) {
        isHumanized = setHumanized;
    }

}
