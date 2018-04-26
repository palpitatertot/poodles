using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Grabber : NetworkBehaviour {

    [SyncVar]
    public bool hasObject = false;
	public bool hasMop = false;
	public bool hasDog = false;

    public GameObject handPosition;

    private AudioSource[] _sfx;

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
        _sfx = GetComponents<AudioSource>();
    }

    void _grab(GameObject go){
        go.transform.parent = gameObject.transform;
        go.transform.position = handPosition.transform.position;
        go.GetComponent<Rigidbody>().isKinematic = true;
        go.GetComponent<NetworkTransform>().transformSyncMode = NetworkTransform.TransformSyncMode.SyncNone;
        go.transform.GetChild(0).gameObject.GetComponent<Collider>().enabled = false;
        InputHandler ih = go.GetComponent<InputHandler>();
		if (ih) {
			ih.setHeld (true);
			hasDog = true;
		} else {						//If is grabbed, and does not have input handler, then it is a mop
			hasMop = true;
		}
        hasObject = true;

    }

    void Grab(){
        if(isServer){
            if (g == null) { return; }
            _grab(g.gameObject);
            RpcGrab(g.gameObject);
        } else{
            CmdGrab();
        }
    }

    void _release(){
        int heldItemLoc = 3;
        if (isLocalPlayer)
        {
            heldItemLoc = 4;
        }

        if (hasMop)
        {
            _sfx[2].pitch = Random.Range(1.0f, 1.2f);
            _sfx[2].Play();
        }

        GameObject go = gameObject.transform.GetChild(heldItemLoc).gameObject;
        go.GetComponent<NetworkTransform>().transformSyncMode = NetworkTransform.TransformSyncMode.SyncRigidbody3D;
        go.GetComponent<Rigidbody>().isKinematic = false;
        go.transform.GetChild(0).GetComponent<Collider>().enabled = true;
        go.gameObject.transform.SetParent(null);
        hasObject = false;
		hasMop = false;
		hasDog = false;
        InputHandler ih = go.GetComponent<InputHandler>();
        if (ih)
        {
            ih.setHeld(false);
        }
    }

    void Release(){
        if (isServer)
        {
            _release();
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
        if (g == null) { return; }
        GameObject go = g.gameObject;
        _grab(go);
        RpcGrab(g.gameObject);
    }

    [Command]
    void CmdRelease(){
        Debug.Log("Command:Releasing");
        _release();
        RpcRelease();
    }

    [ClientRpc]
    void RpcGrab(GameObject go){
        Debug.Log("RPC:Grabbing");
        if (isServer)
        {
            Debug.Log("RPC:Grabbing is server, bailing");
        }
        _grab(go);
    }

    [ClientRpc]
    void RpcRelease(){
        Debug.Log("RPC:Releasing");
        if(isServer){
            Debug.Log("RPC:Releasing is server, bailing");
            return;
        }
        _release();
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
    
    
    
    
    
    

