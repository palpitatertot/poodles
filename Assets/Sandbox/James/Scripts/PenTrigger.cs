using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenTrigger : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        //get the dog the man is holding if he is holding a dog

        Grabber grabber = other.gameObject.transform.parent.GetComponent<Grabber>();

        if (g.hasDog)
        {
            //SpawnController s = grabber.g.gameObject.transform.parent
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
