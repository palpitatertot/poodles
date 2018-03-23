using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Splatter : NetworkBehaviour, ISplatter {

	public float splatScale = 1.0f;
	private int splatsX = 1;
	private int splatsY = 1;
	private Transform _emitter;
	private Vector4 channelMask = new Vector4(0,0,0,1);
    private Vector4 color = new Vector4(0, 0, 0, 0);
    private List<Vector4> colors = new List<Vector4>();
	
	public void SetEmitter(Transform t){
		_emitter = t;
	}

    public void SetColors(List<Vector4> c) {
        foreach (var col in c)
        {
            colors.Add(col);
        }
    }

	public void SetChannel(Vector4 c){
		channelMask = c;
	}

    public void SetColor(Vector4 c){
        color = c;
    }

    public void RegisterSplatter(){
        if(isServer){
            //RpcRegisterSplatter(channelMask, color);
        } else {
            //CmdRegisterSplatter(channelMask, color);
        }
    }

    [Command]
    public void CmdRegisterSplatter(Vector4 channelMask, Vector4 color){
    //    SplatManagerSystem.instance.SetColor(channelMask, color);
    //    RpcRegisterSplatter(channelMask, color);
    }

    [ClientRpc]
    public void RpcRegisterSplatter(Vector4 channelMask, Vector4 color) {
  //      SplatManagerSystem.instance.SetColor(channelMask, color);
    }

    [Command]
    public void CmdSplatCommand(Splat newSplat){
        SplatManagerSystem.instance.AddSplat(newSplat);
        RpcAddSplat(newSplat);
    }
    [ClientRpc]
    public void RpcAddSplat(Splat s)
    {
        SplatManagerSystem.instance.AddSplat(s);    
    }

    void Start(){
        SplatManagerSystem.instance.Colors = colors;
    }

    public void Splat(){          
		RaycastHit hit;
		if( Physics.Raycast( _emitter.position, Vector3.down, out hit, 100 ) ){
			// Get how many splats are in the splat atlas
			splatsX = SplatManagerSystem.instance.splatsX;
			splatsY = SplatManagerSystem.instance.splatsY;
			
			Vector3 leftVec = Vector3.Cross ( hit.normal, Vector3.up );
			float randScale = Random.Range(0.5f,1.5f);
			
			GameObject newSplatObject = new GameObject(); // make this a pool
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

            if (!isServer)
            {
                CmdSplatCommand(newSplat);
            } else {
                RpcAddSplat(newSplat);
            }
			GameObject.Destroy( newSplatObject ); // make this a pool
		}
	}
}