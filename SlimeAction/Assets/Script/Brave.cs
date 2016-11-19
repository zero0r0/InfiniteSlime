using UnityEngine;
using System.Collections;

public class Brave : MonoBehaviour {

    private float speed;

    [SerializeField]
    private GameObject player;
    private Vector3 offset = Vector3.zero;

    private float startPosY;

    private AudioSource audioSource;

    [SerializeField]
    private AudioClip destroySe;

	// Use this for initialization
	void Start () {
        startPosY = this.transform.position.y;
        offset = this.transform.position - player.transform.position;
        audioSource = GetComponent<AudioSource>();
	//    offset = new Vector3(offset.x, this.transform.position.y,0);
	}
	
	// Update is called once per frame
	void LateUpdate () {
        Move();
	}

    void Move()
    {
        Vector3 newPosition = this.transform.position;
        newPosition =  player.transform.position + offset;
        newPosition = new Vector3(newPosition.x, startPosY,newPosition.z);
        transform.position = Vector3.Lerp(transform.position, newPosition, 5.0f * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Obstacle") 
        {
            audioSource.clip = destroySe;
            audioSource.Play();
        }
    }

}
