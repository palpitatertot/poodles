using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Grabber : NetworkBehaviour {


	public bool hasObject = false;
	public GameObject handPosition;

	Grabbable g;

	private bool inRange;

	public void OnTriggerEnter(Collider other){
		inRange =true;
		g = other.gameObject.transform.parent.gameObject.GetComponent<Grabbable> (); 
	}

	public void OnTriggerExit(Collider other){
		inRange = false;
		g = null;
	}
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.E) && !hasObject && inRange) {
			g.gameObject.transform.parent = gameObject.transform;
			g.gameObject.transform.position = handPosition.transform.position;
			g.gameObject.GetComponent<Rigidbody> ().isKinematic = true;
			g.gameObject.transform.GetChild (0).gameObject.GetComponent<Collider> ().enabled = false;
			hasObject = true;
		} else if (Input.GetKeyDown (KeyCode.E) && hasObject) {
			gameObject.transform.GetChild (4).gameObject.GetComponent<Rigidbody> ().isKinematic = false;
			gameObject.transform.GetChild (4).gameObject.transform.GetChild(0).GetComponent<Collider> ().enabled = true;
			gameObject.transform.GetChild (4).gameObject.transform.SetParent(null);
			hasObject = false;
		}
   


    }
	
  
}
    
	
    
    
    
    

