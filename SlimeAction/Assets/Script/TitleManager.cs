using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class TitleManager : MonoBehaviour {

#if UNITY_STANDALONE_WIN
    private int nowSelect = 0;

    private const int START = 0;
    private const int HOWPLAY = 1;
    private const int EXIT = 2;

    private const int SHOW = 3;
    private const int NONE = 4;

    [SerializeField]
    private GameObject selectObj;
    [SerializeField]
    private Transform[] selectPos;

    private int nowPage = 0;
#endif

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
#if UNITY_STANDALONE_WIN
        nowSelect = START;
#endif
        description = false;
        nowDescriptionPage = 0;
	}
	
	// Update is called once per frame
	void Update () {
#if UNITY_ANDROID
        AndroidPlayUpdate();
#elif UNITY_STANDALONE_WIN
        PcPlayUpdate();
#endif
    }

    void AndroidPlayUpdate()
    {
        if (description && Input.GetMouseButtonDown(0))
        {
            if (nowDescriptionPage < howPlaySprite.Length - 1)
            {
                nowDescriptionPage++;
                SoundManager.instance.SoundSystemSE(dicideSe);
                howPlayImage.sprite = howPlaySprite[nowDescriptionPage];
            }
            else
            {
                nowDescriptionPage = 0;
                description = false;
                howPlayImage.gameObject.SetActive(false);
            }
        }
    }

#if UNITY_STANDALONE_WIN 
    void PcPlayUpdate()
    {
        switch (nowSelect)
        {
            case START:
                SelectMenu();
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    GameStart();
                }
                Debug.Log("スタートボタン");
                break;
            case EXIT:
                SelectMenu();
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Application.Quit();
                }
                Debug.Log("やめるボタン");
                break;
            case HOWPLAY:
                SelectMenu();
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SoundManager.instance.SoundSystemSE(dicideSe);
                    howPlayImage.gameObject.SetActive(true);
                    howPlayImage.sprite = howPlaySprite[0];
                    nowSelect = SHOW;
                }
                Debug.Log("遊び方ぼたん");
                break;
            case SHOW:
                ShowHowToPlay();
                Debug.Log("遊び方見せてる");
                break;
        }
    }

    void SelectMenu()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (nowSelect < EXIT)
                nowSelect++;
            else
                nowSelect = START;
            selectObj.transform.localPosition = selectPos[nowSelect].localPosition;
            SoundManager.instance.SoundSystemSE(selectSe);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (START < nowSelect)
                nowSelect--;
            else
                nowSelect = EXIT;
            selectObj.transform.localPosition = selectPos[nowSelect].localPosition;
            SoundManager.instance.SoundSystemSE(selectSe);
        }
    }
#endif

    public void GameStart()
    {
        SoundManager.instance.SoundSystemSE(dicideSe);
        fadeImage.gameObject.SetActive(true);
        iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 1, "time", 1f, "onupdate", "UpdateHandler"));
    }


    public void ShowHowToPlay()
    {
#if UNITY_STANDALONE_WIN
        if (Input.GetKeyDown(KeyCode.RightArrow) && nowPage < howPlaySprite.Length - 1)
        {
            SoundManager.instance.SoundSystemSE(dicideSe);
            nowPage++;
            howPlayImage.sprite = howPlaySprite[nowPage];
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && 0 < nowPage)
        {
            SoundManager.instance.SoundSystemSE(dicideSe);
            nowPage--;
            howPlayImage.sprite = howPlaySprite[nowPage];
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && nowPage == howPlaySprite.Length - 1)
        {
            SoundManager.instance.SoundSystemSE(dicideSe);
            nowPage = 0;
            howPlayImage.gameObject.SetActive(false);
            nowSelect = HOWPLAY;
        }
#elif UNITY_ANDROID
        description = true;
        SoundManager.instance.SoundSystemSE(dicideSe);
        howPlayImage.gameObject.SetActive(true);
        howPlayImage.sprite = howPlaySprite[0];        
#endif
    }

    public void EndGame()
    {
        Application.Quit();
    }

    void UpdateHandler(float value)
    {
        fadeImage.color = new Color(1f,1f,1f,value);
        if (value >= 1)
        {
            SceneManager.LoadScene(1);
            Debug.Log("ロード");
        }
    }

}
