using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// １つのWaveに対応したステージを管理する
/// </summary>
public class Stage : MonoBehaviour {

    private float timer = 0;
    [SerializeField]
    private float intervalTime;
    
    //出現するオブジェクト。
    [SerializeField]
    private GameObject[] obstacle;
    //毒の沼
    [SerializeField]
    private GameObject[] poison;

    //アイテム番号
    private const int NONE = -1;
    private const int MIND = 0;
    private const int NORI = 1;
    private const int DRINK = 2;
    private const int ITEMNUM = 3;
    private GameObject[] item = new GameObject[ITEMNUM];
    private List<int> itemList = new List<int>() {
        MIND, NORI, DRINK
    };  

    //出現ポジション
    private int maxPosX = 2;
    private int minPosX = -2;
    private int maxPosY = 14;
    private int minPosY = 6;

    //障害物同士のＹ間隔
    [SerializeField]
    private float distanceY;

    private void Start() {
        LoadItemPrefab();
    }

    public void UpdateStage() {
        timer += Time.deltaTime;
        if (intervalTime <= timer) {
            StageManager.instance.WaveUp();
        }
    }

    /// <summary>
    /// アイテムはどのステージでも固定なので
    /// リソースからロード
    /// </summary>
    void LoadItemPrefab() {
        item[MIND] = (GameObject)Resources.Load("Prefab/Item/kokoro1");
        item[NORI] = (GameObject)Resources.Load("Prefab/Item/nori");
        item[DRINK] = (GameObject)Resources.Load("Prefab/Item/tubasa_taro");
    }

    /// <summary>
    /// オブジェクトをランダムに生成
    /// </summary>
    /// <param name="parent"></param>
    public void InstantiateRandomObj(Transform parent) {
        for(float y = minPosY; y <= maxPosY; y += distanceY) {
            int rand = Random.Range(0, obstacle.Length);
            int x = Random.Range(minPosX, maxPosX);
            var go = Instantiate(obstacle[rand], new Vector3(x, y, 0), Quaternion.identity) as GameObject;
            go.transform.parent = parent;
            InstantiateItem(parent, y);
        }
        InstantiatePoison(parent);
        //アイテムリストが空になったらリセット
        if (itemList.Count == 0) {
            InitItemList();
        }
    }

    /// <summary>
    /// アイテムランダム生成
    /// アイテムリストから一つ取り出し削除
    /// </summary>
    /// <param name="y"></param>
    void InstantiateItem(Transform parent, float y) {
        //ランダムで決める
        int rand = Random.Range(NONE, itemList.Count);
        int x = Random.Range(minPosX, maxPosX);
        // NONE:生成しない
        if (rand == NONE) {
            return;
        }else {
            //それ以外は指定したアイテム
            Debug.Log(itemList.Count);
            var go = Instantiate(item[rand], new Vector3(x, y, 0), Quaternion.identity) as GameObject;
            go.transform.parent = parent;
            itemList.RemoveAt(rand);
        }
    }

    /// <summary>
    /// アイテムリストのリセット
    /// </summary>
    void InitItemList() {
        itemList = new List<int>() {
            MIND, NORI, DRINK
        };
    }

    /// <summary>
    /// ランダムな位置に毒の沼を出す
    /// </summary>
    /// <param name="parent"></param>
    void InstantiatePoison(Transform parent) {
        int rand = Random.Range(0, poison.Length);
        int x = Random.Range(minPosX, maxPosX);
        int y = Random.Range(minPosY, maxPosY);
        var go = Instantiate(poison[rand], new Vector3(x,y,0),Quaternion.identity);
        go.transform.parent = parent;
    }

}
