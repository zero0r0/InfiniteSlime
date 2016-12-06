﻿using UnityEngine;
using System.Collections;

public class StageManager : MonoBehaviour {

    private int wave;
    public int Wave
    {
        set
        {
            if(wave < maxWave)
                wave = value;
        }
        get
        {
            return wave;
        }
    }


    [SerializeField]
    private int maxWave;

    [SerializeField]
    private int[] wavePoint;

    private Vector2[,] objSpawPos;

    [SerializeField]
    private Vector2 offset;
    [SerializeField]
    private Vector2 startSpawnPos;
    [SerializeField]
    private Vector2 lastSpawnPos;

    [SerializeField]
    private GameObject[] obstacle;
    [SerializeField]
    private GameObject[] item;
    [SerializeField]
    private GameObject mind;

    [SerializeField]
    private Player player;

    [SerializeField]
    private float speed = 1f;

    private float timer = 0;
    [SerializeField]
    private const float INTERVAL = 15f;

    [SerializeField]
    private SpriteRenderer[] background;

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
    private float colorChangeSpeed = 2;
    [SerializeField]
    private float darkTime;

    [SerializeField]
    private Sprite[] backgroundSprites;
    [SerializeField]
    private GameObject[] poisonGround;

    [SerializeField]
    private int maxObstacleNum;

	// Use this for initialization
	void Start () {
        wave = 1;
        SetSpawnList();
        timer = 0;
        isPlaying = false;
        //SpawnObj();
        Debug.Log(background[0].color);
	}

    void Update()
    {
        if (isPlaying)
        {
            timer += speed * Time.deltaTime;
            if (wave < maxWave && timer >= INTERVAL * wave)
            {
                WaveUp();
            }
        }
    }

    void WaveUp()
    {
        StartCoroutine(DarkenBackground());
        maxObstacleNum += 2;
        timer = 0;
        wave++;
        //Debug.Log(wave);
    }
    /// <summary>
    /// 背景を暗くする関数（まだ未実装）
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
    /// <summary>
    /// 障害物やアイテムを発生させて、背景と親子関係を持たせる関数
    /// </summary>
    /// <param name="background"></param>
    public void SpawnObj(Transform background)
    {
        int x = 0;
        for(int i = 0; i < objSpawPos.GetLength(1); i += (maxWave+2) - wave){
            x = Random.Range(0, objSpawPos.GetLength(0));
            int randObj = Random.Range(0, maxObstacleNum);
            var gameObj = Instantiate(obstacle[randObj], objSpawPos[x, i], Quaternion.identity) as GameObject;
            gameObj.transform.parent = background;
        }
        RandomSpawnObj(mind, background);

        int randItem = Random.Range(0, 2);
        if (randItem == 0)
        {
            RandomSpawnObj(item[Random.Range(0, item.Length)], background);
        }

        int randPoison = Random.Range(0,3);
        if (randPoison == 0)
        {
            RandomSpawnObj(poisonGround[Random.Range(0,poisonGround.Length)], background);
        }
    }

    /// <summary>
    /// 指定したオブジェクトをランダムなポジションへ出す関数
    /// </summary>
    /// <param name="spawnGameObject"></param>
    /// <param name="background"></param>
    private void RandomSpawnObj(GameObject spawnGameObject, Transform background)
    {
        int x = Random.Range(0, objSpawPos.GetLength(0));
        int y = Random.Range(0, objSpawPos.GetLength(1));
        var go = Instantiate(spawnGameObject, objSpawPos[x,y], Quaternion.identity) as GameObject;
        go.transform.parent = background;
    }


    /// <summary>
    /// インスペクターで設定した範囲、幅のマスを設定し
    /// それをオブジェクトが現れるポジションとして保存する関数
    /// </summary>
    private void SetSpawnList()
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
    }

}
