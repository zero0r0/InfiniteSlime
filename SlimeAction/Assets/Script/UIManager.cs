using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class UIManager : MonoBehaviour {

    public static UIManager instance = null;

    [SerializeField]
    private Image[] distanceImage;
    [SerializeField]
    private Image[] scoreImage;
    [SerializeField]
    private Image[] mindPercentImage;

    [SerializeField]
    private Sprite[] numFontSprite;

    [SerializeField]
    private Image hpImage;
    [SerializeField]
    private Image hpResult;
    [SerializeField]
    private Image[] mindResultPercent;
    [SerializeField]
    private Image[] distanceResult;

    public Button HumanButton;
    public Button FrontButton;

    public int testScore;

    private float score = 0;

    private bool isPlaying;
    public bool IsPlaying
    {
        get
        {
            return isPlaying;
        }
        set
        {
            isPlaying = value;
        }
    }

    [SerializeField]
    private float speed = 1f;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        isPlaying = false;
        int highScore = ScoreManager.instance.LoadScore();
        SetHighScoreUI(highScore);
    }

    void Update()
    {
        if (isPlaying)
        {
            score += speed * Time.deltaTime;
            SetDistanceUI((int)score);
        }
    }
	
    public void SetHPUI(int hp)
    {
        hpImage.sprite = numFontSprite[hp];
    }

    public void SetDistanceUI(int distance)
    {
        int digit = GetNumberDigit(distance);
        for (int i = 0; i < digit; i++)
        {
            distanceImage[i].sprite = numFontSprite[distance%10];
            distance /= 10;
        }
    }

    public void SetHighScoreUI(int score)
    {
        int digit = GetNumberDigit(score);
       // Debug.Log(digit);
        for (int i = 0; i < digit; i++)
        {
            scoreImage[i].sprite = numFontSprite[score % 10];
            score /= 10;
        }
    }

    public void SetMindUI(int n)
    {
        for (int i = 0; i < mindPercentImage.Length; i++)
        {
            mindPercentImage[i].sprite = numFontSprite[n % 10];
            n /= 10;
        }
    }

    private int GetNumberDigit(int number)
    {
        int digit = 0;
        
        for (int n = number; 0 < n; digit++)
        {
            n /= 10;
        }

        return digit;
    }

    public void SaveHighScore()
    {
        ScoreManager.instance.SaveScore((int)score);
    }

    public void SetResult()
    {
        hpResult.sprite = hpImage.sprite;

        for (int i = 0; i < mindResultPercent.Length; i++)
        {
            mindResultPercent[i].sprite = mindPercentImage[i].sprite;
        }

        
        for (int i = 0; i < distanceResult.Length; i++)
        {
            distanceResult[i].sprite = distanceImage[i].sprite;
        }
    }

}
