﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Splatter : NetworkBehaviour
{
    private Dictionary<Splatter, Drinker> splatters = new Dictionary<Splatter, Drinker>();
    private GameObject newSplatObject;
    public Material DogMat0;
    public Material DogMat1;
    public Material DogMat2;

    private int _connectionId;
    public const short RequestDogColorsMsgId = 101;
    public const short DogColorsMsgId = 201;

    public float splatScale = 1.0f;
    private int splatsX = 1;
    private int splatsY = 1;
    private Transform _emitter;

    [SyncVar]
    private Vector4 channelMask = new Vector4(0, 0, 0, 1);

    private List<Vector4> splatColors = new List<Vector4>();

    public void SetConnectionId(int id){
        _connectionId = id;
    }

    public void SetEmitter(Transform t)
    {
        _emitter = t;
    }

    public void SetColors(List<Vector4> c)
    {
        foreach (var col in c)
        {
            splatColors.Add(col);
        }
    }

    public void SetChannel(Vector4 c)
    {
        channelMask = c;
    }

    public void SetColor(Vector4 c)
    {
        //color = c;
    }


    [Command]
    public void CmdSplatCommand(Splat newSplat)
    {
        if(!ValidateSplat(newSplat)){
            return;
        }
        SplatManagerSystem.instance.AddSplat(newSplat);
        RpcAddSplat(newSplat);
    }
    [ClientRpc]
    public void RpcAddSplat(Splat s)
    {
        SplatManagerSystem.instance.AddSplat(s);
    }

    private void OnDogColorsRequested(NetworkMessage msg) {
        DogColorsMsg response = new DogColorsMsg();
        Debug.Log("Dog Color Request Received.");
        int id = msg.conn.connectionId;
        GameObject[] p = GameObject.FindGameObjectsWithTag("Player");

        for (int j = 0; j < p.Length; j++)
        {
            if(p[j] != null){
                Splatter s = p[j].GetComponent<Splatter>();
                if (s != null && s._connectionId == id)
                {
                    response.Channel = s.channelMask;
                    if(!splatters.ContainsKey(s)) {
                        splatters.Add(s, s.gameObject.GetComponent<Drinker>());
                    }

                }
            }
        }

        int i = 0;
        foreach(Vector4 c in splatColors){
            if (i == 0) { response.Dog0Color = c; }
            if (i == 1) { response.Dog1Color = c; }
            if (i == 2) { response.Dog2Color = c; }
            i++;
        }
        NetworkServer.SendToClient(id, DogColorsMsgId, response);
    }

    private void OnDogColorsReceived(NetworkMessage msg)
    {
        DogColorsMsg message = msg.ReadMessage<DogColorsMsg>();
        Debug.Log("Dog Colors Received.");
        splatColors = new List<Vector4>();
        Debug.Log(message.Dog0Color);
        Debug.Log(message.Dog1Color);
        Debug.Log(message.Dog2Color);
        splatColors.Add(message.Dog0Color);
        splatColors.Add(message.Dog1Color);
        splatColors.Add(message.Dog2Color);
        SplatManagerSystem.instance.Colors = new List<Vector4>();
        SplatManagerSystem.instance.Colors.Add(message.Dog0Color);
        SplatManagerSystem.instance.Colors.Add(message.Dog1Color);
        SplatManagerSystem.instance.Colors.Add(message.Dog2Color);


        Debug.Log("Old Channel: " + channelMask + " New Channel " + message.Channel);
        channelMask = message.Channel;

        SplatManager.SendColorsToRenderer();

        SetDogColors();
    }


    public void TrySplat(Splat newSplat){
        if (!isServer)
        {
            CmdSplatCommand(newSplat);
        }
        else
        {
            if(!ValidateSplat(newSplat)){
                return;
            }
            RpcAddSplat(newSplat);
        }
    }

    public bool ValidateSplat(Splat newSplat){
        Debug.Log("Seeing if peeing?");
        foreach (Splatter s in splatters.Keys)
        {
			if (newSplat.channelMask != SplatChannel.MAN && s.channelMask == newSplat.channelMask) {
				Drinker d = s.gameObject.GetComponent<Drinker> ();
				Debug.Log ("Found me to pee");
				if (!d.CanPee ()) {
					Debug.Log ("Can't pee");
					return false;
				} else {
					Debug.Log ("Peeing");
					d.Pee ();
				}
			} 
			else if (newSplat.channelMask == SplatChannel.MAN && s.channelMask == newSplat.channelMask) 
			{
				Grabber g = s.gameObject.GetComponent<Grabber> ();
				if (g.hasMop) {
					Debug.Log ("Man is splatting");
					return true;
				} else {
					Debug.Log ("Man is not splatting");
					return false;
				}
			}
        }
        return true;
    }

    void SetDogColors(){
        GameObject[] p = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log(p.Length + " Players found.");
        for (int j = 0; j < p.Length; j++)
        {
            if (p[j] != null)
            {
                Debug.Log("Player " + j);
                Splatter s = p[j].GetComponent<Splatter>();
                if (s != null && s.channelMask != SplatChannel.MAN)
                {
                    Debug.Log("Player " + j + "is a dog");
                    Renderer r = p[j].transform.GetChild(0).GetChild(0).GetComponent<Renderer>();
                            
                    if (s.channelMask == SplatChannel.DOG0)
                    {
                        Debug.Log("Player " + j + "is a dog 0");
                        r.material = DogMat0;
                        r.material.color = SplatManagerSystem.instance.Colors[0];
                        Debug.Log(r.material.color);
                    }
                    if (s.channelMask == SplatChannel.DOG1)
                    {
                        Debug.Log("Player " + j + "is a dog 1");
                        r.material = DogMat1;
                        r.material.color = SplatManagerSystem.instance.Colors[1];
                        Debug.Log(r.material.color);
                    }
                    if (s.channelMask == SplatChannel.DOG2)
                    {
                        Debug.Log("Player " + j + "is a dog 2");
                        r.material = DogMat2;
                        r.material.color = SplatManagerSystem.instance.Colors[2];
                        Debug.Log(r.material.color);
                    }
                }
            }
        }
    }

    void Start()
    {
        newSplatObject = new GameObject();

        if (isServer)
        {
            splatters.Add(GetComponent<Splatter>(), GetComponent<Drinker>());
            NetworkServer.RegisterHandler(RequestDogColorsMsgId, OnDogColorsRequested);
       
            SplatManagerSystem.instance.Colors = splatColors;
            SetDogColors();
        }
        else
        {
            GetComponent<NetworkBehaviour>().connectionToServer.RegisterHandler(DogColorsMsgId, OnDogColorsReceived);
        
            RequestDogColorsMsg msg = new RequestDogColorsMsg();
            msg.hi = "hi";
            GetComponent<NetworkBehaviour>().connectionToServer.Send(RequestDogColorsMsgId, msg);
        }
    }

    public void Splat(){          
		RaycastHit hit;
		if( Physics.Raycast( _emitter.position, Vector3.down, out hit, 100 ) ){
			// Get how many splats are in the splat atlas
			splatsX = SplatManagerSystem.instance.splatsX;
			splatsY = SplatManagerSystem.instance.splatsY;
			
			Vector3 leftVec = Vector3.Cross ( hit.normal, Vector3.up );
			float randScale = Random.Range(0.5f,1.5f);
			
			//GameObject newSplatObject = new GameObject(); // make this a pool
			newSplatObject.transform.position = hit.point;
			if( leftVec.magnitude > 0.001f ){
				newSplatObject.transform.rotation = Quaternion.LookRotation( leftVec, hit.normal );
			}
			newSplatObject.transform.RotateAround( hit.point, hit.normal, Random.Range(-180, 180 ) );
			newSplatObject.transform.localScale = new Vector3( randScale, randScale * 0.5f, randScale ) * splatScale;

			Splat newSplat;
			newSplat.splatMatrix = newSplatObject.transform.worldToLocalMatrix;
			newSplat.channelMask = channelMask;

			float splatscaleX = 1.0f / splatsX;
			float splatscaleY = 1.0f / splatsY;
			float splatsBiasX = Mathf.Floor( Random.Range(0,splatsX * 0.99f) ) / splatsX;
			float splatsBiasY = Mathf.Floor( Random.Range(0,splatsY * 0.99f) ) / splatsY;

			newSplat.scaleBias = new Vector4(splatscaleX, splatscaleY, splatsBiasX, splatsBiasY );

            TrySplat(newSplat);
            
			//GameObject.Destroy( newSplatObject ); // make this a pool
		}
	}
}