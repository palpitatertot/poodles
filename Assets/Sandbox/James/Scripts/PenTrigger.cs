﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenTrigger : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered Pen");

        Grabber grabber = other.gameObject.transform.GetComponent<Grabber>();

        if(!grabber){
            return;
        }
        Debug.Log("MAAAAAAN Entered Pen");
        if (grabber.hasDog)
        {
            Debug.Log("MANANA HAS DOGGO");
            SpawnController s = grabber.dog.GetComponent<SpawnController>();
            if(s){
                s.BeginRespawn();
                grabber.DropDog();
                Debug.Log("MANANA DROPPOD DOGGO");
            }
        }
        //get man's grabber
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
