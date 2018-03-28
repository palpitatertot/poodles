using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Grabber : NetworkBehaviour {

    [SyncVar]
	public bool hasObject = false;

	public GameObject handPosition;

	Grabbable g;

    [SyncVar]
	private bool inRange;

	public void OnTriggerEnter(Collider other){
        if(isServer){
            inRange = true;    
            g = other.gameObject.transform.parent.gameObject.GetComponent<Grabbable> (); 
        }
		
	}

	public void OnTriggerExit(Collider other){
        if(isServer){
            inRange = false;
            g = null;    
        }
		
	}
	// Use this for initialization
	void Start () {
		
	}

    void Grab(){
        if(isServer){
            if (g == null) { return; }
            g.gameObject.transform.parent = gameObject.transform;
            g.gameObject.transform.position = handPosition.transform.position;
            //g.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            g.gameObject.transform.GetChild(0).gameObject.GetComponent<Collider>().enabled = false;
            hasObject = true;
            RpcGrab(g.gameObject);
        } else{
            //if (g == null) { return; }
            CmdGrab();
        }
    }

    void Release(){
        if (isServer)
        {
            int heldItemLoc = 3;
            if (isLocalPlayer)
            {
                heldItemLoc = 4;
            }
            gameObject.transform.GetChild(4).gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gameObject.transform.GetChild(4).gameObject.transform.GetChild(0).GetComponent<Collider>().enabled = true;
            gameObject.transform.GetChild(4).gameObject.transform.SetParent(null);
            hasObject = false;
            RpcRelease();
        }
        else
        {
            CmdRelease();
        }
    }


    [Command]
    void CmdGrab(){
        Debug.Log("Command:Grabbing");
        g.gameObject.transform.parent = gameObject.transform;
        g.gameObject.transform.position = handPosition.transform.position;
        g.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        g.gameObject.transform.GetChild(0).gameObject.GetComponent<Collider>().enabled = false;
        hasObject = true;
        RpcGrab(g.gameObject);
    }

    [Command]
    void CmdRelease(){
        Debug.Log("Command:Releasing");
        int heldItemLoc = 3;
        if (isLocalPlayer)
        {
            heldItemLoc = 4;
        }
        gameObject.transform.GetChild(heldItemLoc).gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.transform.GetChild(heldItemLoc).gameObject.transform.GetChild(0).GetComponent<Collider>().enabled = true;
        gameObject.transform.GetChild(heldItemLoc).gameObject.transform.SetParent(null);
        hasObject = false;
        RpcRelease();
    }

    [ClientRpc]
    void RpcGrab(GameObject go){
        Debug.Log("RPC:Grabbing");
        if (isServer)
        {
            Debug.Log("RPC:Grabbing is server, bailing");
        }
        go.transform.parent = gameObject.transform;
        go.transform.position = handPosition.transform.position;
        //GetComponent<Rigidbody>().isKinematic = true;
        go.transform.GetChild(0).gameObject.GetComponent<Collider>().enabled = false;
    }

    [ClientRpc]
    void RpcRelease(){
        Debug.Log("RPC:Releasing");
        if(isServer){
            Debug.Log("RPC:Releasing is server, bailing");
        }
        int heldItemLoc = 3;
        if(isLocalPlayer){
            heldItemLoc = 4;
        }
        gameObject.transform.GetChild(heldItemLoc).gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.transform.GetChild(heldItemLoc).gameObject.transform.GetChild(0).GetComponent<Collider>().enabled = true;
        gameObject.transform.GetChild(heldItemLoc).gameObject.transform.SetParent(null);
    }

	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.E) && !hasObject && inRange) {
            Grab();
			
		} else if (Input.GetKeyDown (KeyCode.E) && hasObject) {
            Release();
		}
   


    }
	
  
}
    
	
    
    
    
    

