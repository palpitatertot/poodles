using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Drinkable : NetworkBehaviour {
    
    public int WaterPerSecond;
    private List<Drinker> inRange = new List<Drinker>();    

    private void Start()
	{        
        StartCoroutine(Drinking());
	}
	private void OnTriggerEnter(Collider other)
	{              
        if(!isServer){
            Debug.Log("Entered as Client");
            return;
        }
        Debug.Log("Entered as Server");
        Drinker d = other.gameObject.transform.parent.gameObject.GetComponent<Drinker>();
        Debug.Log(other.gameObject.transform.parent.gameObject);
        Debug.Log(d);
        if(d != null){
            Debug.Log("D Exists");
            inRange.Add(d);
        }
        Debug.Log(inRange.Count);               
	}

    private void OnTriggerExit(Collider other)
    {        
        TurnOffSound();

        if (!isServer)
        {
            Debug.Log("Leaved as Client");
            return;
        }
        Debug.Log("Leaved as Server");
        Drinker d = other.gameObject.transform.parent.gameObject.GetComponent<Drinker>();
        if (d != null)
        {
            Debug.Log("D Exists");
            inRange.Remove(d);
        }
        Debug.Log(inRange.Count);
    }

    public void TurnOffSound()
    {
        foreach (Drinker d in inRange)
        {
            d.TurnOffSound();
        }
    }

    IEnumerator Drinking(){
        while(true){
            foreach(Drinker d in inRange){
                d.Drink(WaterPerSecond);    
            }
            yield return new WaitForSeconds(1);    
        }
    }
}
