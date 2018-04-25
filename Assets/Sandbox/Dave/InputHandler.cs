﻿using System.Collections;
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
		if (!_spawning) {
			dInput.fx = Input.GetAxis ("Horizontal") * FrontTurnSpeed * Time.deltaTime;
			dInput.fz = Input.GetAxis ("Vertical") * FrontRunSpeed * Time.deltaTime;
			dInput.rx = Input.GetAxis ("Horizontal2") * FrontTurnSpeed * Time.deltaTime;
			dInput.rz = Input.GetAxis ("Vertical2") * FrontRunSpeed * Time.deltaTime;

            //if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.LeftArrow)
            //|| Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.RightArrow)
            //|| Input.GetKeyDown(KeyCode.DownArrow))
            //{
            //    if (_sfx[2].isPlaying)
            //    {
            //        _sfx[2].pitch = Random.Range(1.0f, 1.2f); //walking sounds
            //        _sfx[2].Play();
            //    }                
            //}

            //if (!Input.anyKey)
            //{
            //    _sfx[2].Stop();
            //}

            //if (Input.GetKeyUp(KeyCode.Space))
            //{
            //    _sfx[4].Stop();
            //}

            //  if (isMoving())
            //  {
            //      if(!_startWalking)
            //      {
            //          _sfx[2].pitch = Random.Range(1.0f, 1.2f); //walking sounds
            //          _sfx[2].Play();
            //         // _sfx[2].PlayOneShot(_sfx[2].clip);
            //      }
            //  }
            //
            //  if (!isMoving())
            //  {
            //      _startWalking = false;
            //      _sfx[2].Stop();
            //  }

            //if (Input.GetKeyDown(KeyCode.Space)) //pee sounds
            //{
            //    _sfx[4].pitch = Random.Range(1.0f, 1.2f); //ee sounds
            //    //_sfx[4].PlayOneShot(_sfx[4].clip);
            //    _sfx[4].Play();
            //}

            //if (Input.GetKeyUp(KeyCode.Space))
            //{
            //    _sfx[4].Stop();
            //}

            if (Input.GetKey (KeyCode.Space)) {
				dInput.splat = true;
               
            } else {
				dInput.splat = false;
            }
		} else {
			dInput.fx = 0;
			dInput.fz = 0;
			dInput.rx = 0;
			dInput.rz = 0;
			dInput.splat = false;

        _spawning = false;
        _held = false;
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

    private bool isMoving()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.LeftArrow)
            || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.RightArrow)
            || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!_startWalking && !_isWalking)
            {
                _startWalking = true;
            }

            _isWalking = true;

            return true;
        }

        return false;
    }

    private void PlaySound(AudioSource source)
    {
        source.pitch = Random.Range(1.0f, 1.2f);
        source.Play();
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
