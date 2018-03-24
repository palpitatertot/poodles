using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Drinker : NetworkBehaviour {

    [SyncVar(hook = "OnWaterChanged")]
    public int WaterLevel = 0;
    public int WaterMax = 100;
    public int PeeCost = 1;

    public void OnWaterChanged(int amount){
        Debug.Log("Dranking level is " + WaterLevel);
        // insert UI Hook
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
