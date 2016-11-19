using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

    [SerializeField]
    private GameObject crashEffect;
    [SerializeField]
    private float effectOffsetY = -0.1f;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player" || col.tag == "Brave")
        {
            Destroy(this.gameObject);
            Instantiate(crashEffect, this.transform.position+new Vector3(0,effectOffsetY,0), Quaternion.identity);
        }
        else if (col.tag == "Mind" || col.tag == "Glue" || col.tag == "Buru")
        {
            Destroy(this.gameObject);
        }

    }
}
