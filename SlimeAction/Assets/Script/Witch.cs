using UnityEngine;
using System.Collections;

public class Witch : MonoBehaviour {

    [SerializeField]
    private GameObject house;
    [SerializeField]
    private GameObject magic;
    [SerializeField]
    private GameObject effect;

    [SerializeField]
    private float speed = 1.5f;
    private float direction;

    enum State
    {
        NONE,MOVE, MAGIC
    }
    State state;

    void Start()
    {
        state = State.NONE;
        if (house.transform.position.x >= 0)
        {
            direction = -1f;
        }
        else
        {
            direction = 1f;
        }
    }

    void OnBecameVisible()
    {
        state = State.MOVE;
    }

	// Update is called once per frame
	void Update () {
        switch (state){
            case State.NONE:

                break;
            case State.MOVE:
                this.transform.position += new Vector3(1f,0,0) * speed * direction * Time.deltaTime;
                if (Vector3.Distance(house.transform.position + new Vector3(direction,0,0),this.transform.position) <= 0.1f)
                {
                    state = State.MAGIC;
                }
                break;
            case State.MAGIC:
                if (magic.transform.localScale.x < 1f)
                    magic.transform.localScale += new Vector3(1f, 1f, 1f) * Time.deltaTime * speed;
                else
                    state = State.NONE;
                break;
        }
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag != "Player" && col.tag != "House" && col.tag != "Brave")
        {
            Instantiate(effect,col.gameObject.transform.position, Quaternion.identity);
            Destroy(col.gameObject);
        }
    }

}
