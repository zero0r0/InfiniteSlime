using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class TitleManager : MonoBehaviour {

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

    [SerializeField]
    private Image fadeImage;

    [SerializeField]
    private Image howPlayImage;
    [SerializeField]
    private Sprite[] howPlaySprite;
    private int nowPage = 0;

    [SerializeField]
    private AudioClip selectSe;
    [SerializeField]
    private AudioClip dicideSe;

	// Use this for initialization
	void Start () {
        nowSelect = START;
	}
	
	// Update is called once per frame
	void Update () {
        switch (nowSelect)
        {
            case START:
                SelectMenu();
                if (Input.GetKeyDown(KeyCode.Space))              
                {
                    SoundManager.instance.SoundSystemSE(dicideSe);
                    GameStart();
                }

                break;
            case EXIT:
                SelectMenu();
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Application.Quit();
                }
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
                break;
            case SHOW:
                ShowHowToPlay();
                break;
        }

	}

    void ShowHowToPlay()
    {
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

    void GameStart()
    {
        fadeImage.gameObject.SetActive(true);
        iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 1, "time", 1f, "onupdate", "UpdateHandler"));
    }

    void UpdateHandler(float value)
    {
        fadeImage.color = new Color(1f,1f,1f,value);
        if (value >= 1)
        {
            SceneManager.LoadScene(1);
        }
    }

}
