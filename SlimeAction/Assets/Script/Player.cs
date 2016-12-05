using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {

    [SerializeField]
    private float wideSpeed;
    [SerializeField]
    private float addWideSpeed;
    [SerializeField]
    private Vector2 addScale;

    //ココロノカケラ
    private int mind;
    public int Mind
    {
        get
        {
            return mind;
        }
        private set {}
    }

    [SerializeField]
    private int addMind;

    [SerializeField]
    private float maxPosX;
    [SerializeField]
    private float minPosX;

    [SerializeField]
    private float playerBackY;
    private bool goFront;

    [SerializeField]
    private int maxHp;
    [SerializeField]
    private int hp;
    public int HP
    {
        get
        {
            return hp;
        }
        private set{}
    }

    private bool isDead;

    /*
    private bool invincibleObstacle;
    private bool invinciblePoison;
    [SerializeField]
    private float invincibleObstacleTime = 1f;
    [SerializeField]
    private float invinceblePoisonTime = 1f;
    */

    [SerializeField]
    private float invincibleTime = 1f;
    private bool invincible = false;

    private bool isWing;
    [SerializeField]
    private float isWingTime = 2f;

    [SerializeField]
    private AudioSource walkAudioSource;
    [SerializeField]
    private AudioSource effectAudioSource;
    
    [SerializeField]
    private AudioClip damageSe;
    [SerializeField]
    private AudioClip mindSe;
    [SerializeField]
    private AudioClip destroySe;
    [SerializeField]
    private AudioClip healingSe;
    [SerializeField]
    private AudioClip poisonSe;
    [SerializeField]
    private AudioClip pickSe;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private GameManager gameManager;

    public ParticleSystem evolution;

    [SerializeField]
    private SpriteRenderer spRenderer;
    private float invincibleTimer = 0;
    [SerializeField]
    private float rendererInterval = 0.1f;


	// Use this for initialization
	void Start () {
        mind = 0;
       // hp = 0;
        isDead = false;
        invincible = false;
        isWing = false;
        goFront = false;
        UIManager.instance.SetHPUI(hp);
        walkAudioSource.Play();
    }
	
	// Update is called once per frame
	void Update () {
        if (!isDead)
        {
            Move();
            GoFront();
            if (invincible)
            {
                Blink();
            }
        }
	}

    /// <summary>
    /// ダメージ時の点滅関数
    /// </summary>
    void Blink()
    {
        invincibleTimer += Time.deltaTime;
        if (rendererInterval < invincibleTimer)
        {
            spRenderer.enabled = !spRenderer.enabled;
            invincibleTimer = 0;
        }
    }
    /// <summary>
    /// 横移動関数
    /// ポジション移動とアニメーション、音
    /// </summary>
    void Move()
    {
        float x = 0;
#if UNITY_STANDALONE_WIN
        x = Input.GetAxis("Horizontal");
#elif UNITY_ANDROID
        Vector3 tapWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButton(0) && tapWorldPos.x > 0)
        {
            x = 1f;
        }
        else if(Input.GetMouseButton(0) && tapWorldPos.x < 0)
        {
            x = -1f;
        }
        else
        {
            x = 0;
        }
#endif
        Vector3 playerPos = this.transform.position;

        this.transform.position += CalculateMovePosition(playerPos.x, x) * Time.deltaTime;
        
        if (x > 0)
        {
            anim.SetBool("Right", true);
            anim.SetBool("Left",false);
            if (walkAudioSource.mute)
            {
                walkAudioSource.mute = false;
            }

        }
        else if (x < 0)
        {
            anim.SetBool("Left", true);
            anim.SetBool("Right",false);
            if (walkAudioSource.mute)
            {
                walkAudioSource.mute = false;
            }
        }
        else
        {
            anim.SetBool("Left", false);
            anim.SetBool("Right", false);
            if (!walkAudioSource.mute)
            {
                walkAudioSource.mute = true;
            }
        }
    }

    /// <summary>
    /// ポジションの移動量を返す
    /// ポジションのｘが最大又は最小に達してたら移動はしない
    /// </summary>
    /// <param name="playerPosX"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    Vector3 CalculateMovePosition(float playerPosX, float x)
    {
        if ((maxPosX < playerPosX && 0 < x )|| (playerPosX < minPosX && x < 0))
        {
            return new Vector3(0, 0, 0);
        }
        else{
            return new Vector3(x * wideSpeed, 0, 0);
        }
    }
    /// <summary>
    /// HPを削って前にでる関数
    /// スケールも小さくし、スピードもUP
    /// </summary>
    void GoFront()
    {
        if (1 < hp && (Input.GetKeyDown(KeyCode.UpArrow) || goFront))
        {
            this.transform.position -= new Vector3(0,playerBackY,0);
            this.transform.localScale -= new Vector3(addScale.x, addScale.y,0);
            wideSpeed += addWideSpeed;
            hp--;
            UIManager.instance.SetHPUI(hp);
            goFront = false;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.tag) {
            case "Mind":
                effectAudioSource.clip = mindSe;
                effectAudioSource.Play();
                mind += addMind;
                if (1000 <= mind)
                {
                    mind = 999;
                }
                UIManager.instance.SetMindUI(mind);
                break;

            case "DamageObj":
                if (!invincible && !isWing)
                {
                    //InvinciblePoison();
                    Invinceble();
                    effectAudioSource.clip = poisonSe;
                    effectAudioSource.Play();
                    this.transform.localScale -= new Vector3(addScale.x, addScale.y, 0);
                    wideSpeed += addWideSpeed;
                    hp--;
                    UIManager.instance.SetHPUI(hp);
                    if (hp <= 0)
                    {
                        isDead = true;
                        gameManager.GameOver();
                    }
                }
                break;
        
            case "Glue":
                if (hp < maxHp)
                {
                    effectAudioSource.clip = healingSe;
                    effectAudioSource.Play();
                    hp++;
                    this.transform.localScale += new Vector3(addScale.x, addScale.y, 0);
                    wideSpeed -= addWideSpeed;
                    UIManager.instance.SetHPUI(hp);
                }
                break;
      
            case "Obstacle":
                if (!invincible){
                    effectAudioSource.clip = destroySe;
                    effectAudioSource.Play();
                    this.transform.position += new Vector3(0, playerBackY, 0);
                    Invinceble();       
                    //InvincibleObstacle();
                }
                break;
            
            case "Buru":
                effectAudioSource.clip = pickSe;
                effectAudioSource.Play();
                if (!isWing) {
                    Wing();
                }
                break;

            case "Brave":
                isDead = true;
                gameManager.GameOver();
                break;

            default:
                break;
        }
        if (col.tag != "Brave" && col.tag != "DamageObj")
        {
            Destroy(col.gameObject);
        }
    }


    void Wing()
    {
        isWing = true;
        Invoke("ReleasedWing", isWingTime);       
    }

    void ReleasedWing()
    {
        isWing = false;
    }

    void Invinceble()
    {
        invincible = true;
        Invoke("ReleasedInvincible",invincibleTime);
    }

    void ReleasedInvincible()
    {
        invincible = false;
        spRenderer.enabled = true;
    }

    public void ClickGoFrontButton()
    {
        if (!goFront)
            goFront = true;
    }


    /*

    void InvincibleObstacle()
    {
        invincibleObstacle = true;
        Invoke("ReleasedInvincibleObstacle", invincibleObstacleTime);
    }

    void InvinciblePoison()
    {
        invinciblePoison = true;
        Invoke("ReleasedInvinciblePoison", invinceblePoisonTime);
    }
    void ReleasedInvincibleObstacle()
    {
        invincibleObstacle = false;
        spRenderer.enabled = true;
    }

    void ReleasedInvinciblePoison()
    {
        invinciblePoison = false;
        spRenderer.enabled = true;
    }
    */

}
