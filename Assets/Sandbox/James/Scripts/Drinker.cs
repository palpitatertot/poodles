using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Drinker : NetworkBehaviour {

    [SyncVar(hook = "OnWaterChanged")]
    public int WaterLevel = 0;
    public int WaterMax = 100;
    public int PeeCost = 1;

    public pauseMenu menu;

	private void Start()
	{
        menu = GameObject.Find("PauseCanvas").GetComponent<pauseMenu>();	
	}

	public void OnWaterChanged(int amount){
        Debug.Log("Dranking level is " + WaterLevel);
        if(isClient){
            WaterLevel = amount;   
        }
        if (isLocalPlayer) {
            menu.updateWaterLevel((float)WaterLevel / WaterMax);   
        }
    }

    public void Drink(int amount){
        if(!isServer){
            return;
        }
        WaterLevel += amount;
        if (WaterLevel > WaterMax)
        {
            WaterLevel = WaterMax;
        }
        else if (WaterLevel < 0){
            WaterLevel = 0;
        }
    }

    public bool CanPee(){
        if (!isServer)
        {
            return false;
        }
        return WaterLevel >= PeeCost;
    }

    public void Pee(){
        if (!isServer)
        {
            return;
        }
        WaterLevel -= PeeCost;
    }
}
