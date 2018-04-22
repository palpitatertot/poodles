using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

	//Input handler captures input and stores it in a struct
	// The Dog Controller will query the input handler for input struct and handle accordingly
	//	When spawning, have input handler report no input

	private float fx, fz, rx, rz, y;
	public float FrontRunSpeed;
	public float RearRunSpeed;
	public float FrontTurnSpeed;
	public float RearTurnSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		fx = Input.GetAxis("Horizontal") * FrontTurnSpeed * Time.deltaTime;
		fz = Input.GetAxis("Vertical") * FrontRunSpeed * Time.deltaTime;
		rx = Input.GetAxis("Horizontal2") * FrontTurnSpeed * Time.deltaTime;
		rz = Input.GetAxis("Vertical2") * FrontRunSpeed * Time.deltaTime;
	}
}
