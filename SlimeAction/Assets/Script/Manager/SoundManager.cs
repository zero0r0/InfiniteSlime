using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    [SerializeField]
    private AudioSource se_source;
    [SerializeField]
    private AudioSource bgm_source;
    public static SoundManager instance = null;

    [SerializeField]
    private AudioClip mainIntroBGM;
    [SerializeField]
    private AudioClip mainBGM;

    private bool isPlaying = true;
    public bool IsPlaying{
        get
        {
            return isPlaying;
        }
        set
        {
            isPlaying = value;
        }
    }

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    /// <summary>
    /// メインゲームのBGM関数だけ独立（二種類あるので）
    /// </summary>
    public void SoundMainGameBGM()
    {
        bgm_source.loop = false;
        bgm_source.clip = mainIntroBGM;
        bgm_source.Play();
        StartCoroutine("CheckIntroBGM");
    }

    private IEnumerator CheckIntroBGM()
    {
        while (bgm_source.isPlaying)
        {
            yield return null;
        }
        if (isPlaying)
        {
            Debug.Log("Loopはいるよ");
            bgm_source.clip = mainBGM;
            bgm_source.Play();
            bgm_source.loop = true;
        }
    }


    /// <summary>
    ///　BGMの再生
    /// </summary>
    /// <param name="clip">
    /// AudioClip
    /// </param>
    public void SoundSystemSE(AudioClip clip)
    {
        se_source.clip = clip;
        se_source.Play();
    }

    
    /// <summary>
    /// BGMの再生
    /// </summary>
    /// <param name="clip">
    /// AudioClip
    /// </param>
    public void SoundBGM(AudioClip clip)
    {
        bgm_source.clip = clip;
        bgm_source.Play();
    }

    /// <summary>
    /// BGMのストップ
    /// </summary>
    public void StopBGM()
    {
        bgm_source.Stop();
    }
}