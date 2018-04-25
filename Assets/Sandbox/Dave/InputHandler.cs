using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct dogInput{
	public float fx, fz, rx, rz, y;
	public bool splat;
}


public class InputHandler : MonoBehaviour {

	//Input handler captures input and stores it in a struct
	// The Dog Controller will query the input handler for input struct and handle accordingly
	//	When spawning, have input handler report no input

	public float FrontRunSpeed;
	public float RearRunSpeed;
	public float FrontTurnSpeed;
	public float RearTurnSpeed;

    private AudioSource[] _sfx;
	private dogInput dInput;

    private bool _isWalking = false;
    private bool _startWalking = false;

	private bool _spawning;
    private bool _held;

	// Use this for initialization
	void Start () {
         _sfx = GetComponents<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        if (_spawning)
        {
            dInput.fx = 0;
            dInput.fz = 0;
            dInput.rx = 0;
            dInput.rz = 0;
            dInput.splat = false;
        }
        else if (_held)
        {
            dInput.fx = 0;
            dInput.fz = 0;
            dInput.rx = 0;
            dInput.rz = 0;
            if (Input.GetKey(KeyCode.Space))
            {
                dInput.splat = true;
            }
            else
            {
                dInput.splat = false;
            }
        }
        else {
            dInput.fx = Input.GetAxis("Horizontal") * FrontTurnSpeed * Time.deltaTime;
            dInput.fz = Input.GetAxis("Vertical") * FrontRunSpeed * Time.deltaTime;
            dInput.rx = Input.GetAxis("Horizontal2") * FrontTurnSpeed * Time.deltaTime;
            dInput.rz = Input.GetAxis("Vertical2") * FrontRunSpeed * Time.deltaTime;

            if (Input.GetKey(KeyCode.Space))
            {
                dInput.splat = true;
            }
            else
            {
                dInput.splat = false;
            }

		}
	}

    public dogInput getInputData()
	{
		return dInput;
	}

	public void setSpawning(bool spawn)
	{
		_spawning = spawn;
	}

    public void setHeld(bool held)
    {
        _held = held;
    }

    public bool isHeld()
    {
        return _held;    
    }
}
