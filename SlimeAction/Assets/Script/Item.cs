using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Obstacle")
        {
            Destroy(this.gameObject);
            //Debug.Log("obstacle");
        }
        
    }

}
