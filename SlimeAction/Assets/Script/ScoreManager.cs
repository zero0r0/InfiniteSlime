using UnityEngine;
using System.Collections;

/// <summary>
/// スコアのセーブロードのみ
/// </summary>
public class ScoreManager : MonoBehaviour {

    const string HIGH_SCORE_KEY = "highScore";

    public static ScoreManager instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

    }

	public void SaveScore (int score) {
        int highScore = LoadScore();
        if (highScore < score)
        {
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, score);
        }
	}
	
	// Update is called once per frame
	public int LoadScore () {
        return PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
	}
}
