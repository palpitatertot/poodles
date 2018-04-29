using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Drinker : NetworkBehaviour {

    [SyncVar(hook = "OnWaterChanged")]
    public int WaterLevel = 0;
    public int WaterMax = 100;
    public int PeeCost = 1;
	private bool _cheater = false;

    public pauseMenu menu;
    private AudioSource[] _sfx;

    private void Start()
	{
        _sfx = GetComponents<AudioSource>();
        menu = GameObject.Find("PauseCanvas").GetComponent<pauseMenu>();	
	}

	void Update()
	{
		if(Input.GetKey(KeyCode.O))
		{
			_cheater = !_cheater;		
		}	

		if (_cheater) {
			WaterLevel = 100;
		}
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

        if (!_sfx[1].isPlaying)
        {
            _sfx[1].pitch = Random.Range(1.0f, 1.2f);
            _sfx[1].Play();
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

    public void TurnOffSound()
    {
        if (_sfx[1].isPlaying)
        {
            _sfx[1].Stop();
        }
    }
}
