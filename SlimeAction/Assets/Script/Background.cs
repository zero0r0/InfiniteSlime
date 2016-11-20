﻿using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {

    [SerializeField]
    private float StartPosY;
    [SerializeField]
    private float EndPosY;
    [SerializeField]
    private float speed;

    [SerializeField]
    private StageManager stageManager;

	// Use this for initialization
	void Start () {
        	    
	}
	
	// Update is called once per frame
	void Update () {
        Move();
    }

    void Move()
    {
        this.transform.position -= Vector3.up * speed * Time.deltaTime;
        if (this.transform.position.y <= EndPosY)
        {
            ScrollEnd();
        }

    }

    void ScrollEnd()
    {
        transform.Translate(0,-1 * (EndPosY - StartPosY),0);
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        //Debug.Log(n);
        stageManager.SpawnObj(this.transform);
        //SendMessage("OnScrollEnd", SendMessageOptions.DontRequireReceiver);
    }
}