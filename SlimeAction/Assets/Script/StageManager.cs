using UnityEngine;
using System.Collections;

/// <summary>
/// ステージのWaveなどの全体を管理
/// </summary>
public class StageManager : MonoBehaviour {

    public static StageManager instance = null;

    //private Vector2[,] objSpawPos;

    //ステージ関連
    [SerializeField]
    private Stage[] stage;
    private int wave;
    public int Wave {
        get {
            return wave;
        }
        set {
            if (wave < maxWave)
                wave = value;
        }
    }
    [SerializeField]
    private int maxWave;


    [SerializeField]
    private Player player;

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

    //背景関連
    [SerializeField]
    private SpriteRenderer[] background;
    [SerializeField]
    private float colorChangeSpeed = 2;
    [SerializeField]
    private float darkTime;
    [SerializeField]
    private Sprite[] backgroundSprites;

    void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }


    // Use this for initialization
    void Start () {
        wave = 0;
        //SetSpawnList();
        isPlaying = false;
        //SpawnObj();
       // Debug.Log(background[0].color);
	}

    void Update()
    {
        if (isPlaying)
        {
            stage[wave].UpdateStage();
        }
    }

    public void WaveUp()
    {
        if (wave < maxWave) {
            Wave++;
            StartCoroutine(DarkenBackground());
        }
        //Debug.Log(wave);
    }

    /// <summary>
    /// 背景を暗くした後スプライトを変えて明るくする関数
    /// </summary>
    /// <returns></returns>
    private IEnumerator DarkenBackground()
    {
        Color backgroundColor = background[0].color;
        while (backgroundColor.a > 0.6)
        {
            backgroundColor -= new Color(1, 1, 1, 1) * colorChangeSpeed * Time.deltaTime;
            for (int i = 0; i < background.Length; i++)
            {
                background[i].color = backgroundColor;
            }
            //Debug.Log("暗くしてるよ");
            yield return null;
        }

        yield return new WaitForSeconds(darkTime);

        for (int i = 0; i < background.Length; i++)
        {
            background[i].sprite = backgroundSprites[wave-1];
        }
        //Debug.Log("チェンジ！");
        while (backgroundColor.a < 1)
        {
            backgroundColor += new Color(1, 1, 1, 1) * colorChangeSpeed * Time.deltaTime;
            for (int i = 0; i < background.Length; i++)
            {
                background[i].color = backgroundColor;
            }
            //Debug.Log("明るく");
            yield return null;
        }
    }

    public void InstantiateObj(Transform parent) {
        stage[Wave].InstantiateRandomObj(parent);
    }

    /// <summary>
    /// インスペクターで設定した範囲、幅のマスを設定し
    /// それをオブジェクトが現れるポジションとして保存する関数
    /// </summary>
    /*private void SetSpawnList()
    {
        int x = Mathf.Abs((int)((lastSpawnPos.x - startSpawnPos.x) / offset.x));
        int y = Mathf.Abs((int)((lastSpawnPos.y - startSpawnPos.y) / offset.y));
        objSpawPos = new Vector2[x, y];
        
        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                objSpawPos[j,i] = startSpawnPos + new Vector2(j * offset.x, i * offset.y);
            }
        }
        Debug.Log(objSpawPos.GetLength(0));
        Debug.Log(objSpawPos.GetLength(1));
    }*/

}
