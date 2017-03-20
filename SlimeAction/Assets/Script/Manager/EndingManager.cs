using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// エンディングの判定、ED、Gameover画像のセット
/// </summary>
public class EndingManager : MonoBehaviour {

    [SerializeField]
    private Image flashImage;

    [SerializeField]
    private GameObject endingGroup;
    [SerializeField]
    private Image endingImage;
    [SerializeField]
    private GameObject endingScore;
    [SerializeField]
    private Sprite[] badEndingSprite;
    [SerializeField]
    private Sprite[] clearEndingSprite;

    [SerializeField]
    private int[] endingMindPoint;

    [SerializeField]
    private float alphaSpeed = 0.1f;

    public static EndingManager instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

    }

    /// <summary>
    /// エンディングの判定
    /// 死亡フラグやHP、ココロノカケラの数でどの画像を表示するか判定
    /// </summary>
    /// <param name="isDead">死んでるかどうか</param>
    /// <param name="mindNum">ココロノカケラ数</param>
    public void JudgeEnding(bool isDead , int mindNum , int hp = 1) {

        Sprite endingSprite = null;

        if (isDead)
        {
            foreach (Transform child in endingGroup.transform)
            {
                child.gameObject.SetActive(false);
            }
            //endingSprite = badEndingSprite;
            if (hp == 0)
            {
                endingGroup.GetComponent<Image>().color = Color.white;
                endingGroup.GetComponent<Image>().sprite = badEndingSprite[1];
            }
            else
            {
                endingGroup.GetComponent<Image>().color = Color.white;
                endingGroup.GetComponent<Image>().sprite = badEndingSprite[0];
            }
        }
        else
        {
            for (int i = 0; i < endingMindPoint.Length; i++)
            {
                if (endingMindPoint[i] <= mindNum/* && mindNum < endingMindPoint[i + 1]*/)
                {
                    endingSprite = clearEndingSprite[i];
                }
            }
            endingImage.gameObject.SetActive(true);
            endingScore.gameObject.SetActive(true);
        }
        endingImage.sprite = endingSprite;
        endingGroup.SetActive(true);
        //StartCoroutine("IndicateEndingImage");
    }

    /*
  public IEnumerator IndicateEndingImage()
  {
      for (float f = 0f; f < 1; f += alphaSpeed * Time.deltaTime)
      {
          Color c = endingImage.color;
          c.a = f;
          endingImage.color = c;
          yield return null;
      }
  }
  */


  
    /// <summary>
    /// フラッシュ（ゲームクリア、オーバー時のフェイドインフェイドアウト）
    /// </summary>
    /// <param name="color"></param>
    /// <param name="startAlpha"></param>
    /// <param name="endAlpha"></param>
    /// <param name="sigh"></param>
    /// <returns></returns>
    public IEnumerator FlashImage(Color color,float startAlpha, float endAlpha)
    {
        flashImage.gameObject.SetActive(true);
        float timer = 0;
        Color startColor = color;
        Color endColor = color;
        startColor.a = startAlpha;
        endColor.a = endAlpha;
        while (timer < 1)
        {
            timer += alphaSpeed * Time.deltaTime;
            flashImage.color = Color.Lerp(startColor, endColor, timer);
            yield return null;
        }
    }

}
