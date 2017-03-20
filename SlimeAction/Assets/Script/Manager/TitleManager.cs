using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class TitleManager : MonoBehaviour {

    [SerializeField]
    private Image fadeImage;

    [SerializeField]
    private Image howPlayImage;
    [SerializeField]
    private Sprite[] howPlaySprite;
    private int nowDescriptionPage;
    private bool description;

    [SerializeField]
    private AudioClip selectSe;
    [SerializeField]
    private AudioClip dicideSe;

	// Use this for initialization
	void Start () {
        iTween.ValueTo(gameObject, iTween.Hash("from", 1, "to", 0, "time", 1f, "onupdate", "FadeInUpdateHandler"));
        description = false;
        nowDescriptionPage = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (description && Input.GetMouseButtonDown(0)) {
            if (nowDescriptionPage < howPlaySprite.Length - 1) {
                nowDescriptionPage++;
                SoundManager.instance.SoundSystemSE(dicideSe);
                howPlayImage.sprite = howPlaySprite[nowDescriptionPage];
            }
            else {
                nowDescriptionPage = 0;
                description = false;
                howPlayImage.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 遊び方ボタンイベントから呼び出し
    /// </summary>
    public void ShowHowToPlay()
    {
        description = true;
        SoundManager.instance.SoundSystemSE(dicideSe);
        howPlayImage.gameObject.SetActive(true);
        howPlayImage.sprite = howPlaySprite[0];        
    }

    /// <summary>
    /// ゲーム終了ボタンから呼び出し
    /// </summary>
    public void EndGame()
    {
        Application.Quit();
    }


    /// <summary>
    /// スタートボタンのイベントから呼び出し
    /// </summary>
    public void StartGame()
    {
        SoundManager.instance.SoundSystemSE(dicideSe);
        fadeImage.gameObject.SetActive(true);
        iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 1, "time", 1f, "onupdate", "FadeOutUpdateHandler"));
    }

    void FadeOutUpdateHandler(float value)
    {
        fadeImage.color = new Color(1f,1f,1f,value);
        if (value >= 1)
        {
            SceneManager.LoadScene(1);
            Debug.Log("ロード");
        }
    }
    void FadeInUpdateHandler(float value)
    {
        fadeImage.color = new Color(1f, 1f, 1f, value);
        if (value <= 0) 
        {
            fadeImage.gameObject.SetActive(false);
        }
    }

}
