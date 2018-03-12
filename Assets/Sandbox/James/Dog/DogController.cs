﻿using UnityEngine;
using UnityEngine.Networking;

public class DogController : MonoBehaviour
{
	public float FrontRunSpeed;
	public float RearRunSpeed;
	public float FrontTurnSpeed;
	public float RearTurnSpeed;
	public float MouseSpeed;

	private Transform _camera;
	private Transform _frontPivot;
	private Transform _rearPivot;
	private Rigidbody _front;
	private Rigidbody _rear;
	private float fx, fz, rx, rz, y;

	void Start()
	{
		_camera = transform.GetChild (0);	
		_camera.position = transform.position + new Vector3(0,20,0);
		_camera.LookAt(transform);
		_frontPivot = transform.GetChild(2);
		_rearPivot = transform.GetChild(3);
		//_front = _frontPivot.GetComponent<Rigidbody> ();
		//_rear = _rearPivot.GetComponent<Rigidbody> ();
		_front = GetComponent<Rigidbody> ();
	}

	void Update()
	{
		//if (!isLocalPlayer)
		//{
	//		return;
	//	}
	
		fx = Input.GetAxis("Horizontal") * FrontTurnSpeed * Time.deltaTime;
		fz = Input.GetAxis("Vertical") * FrontRunSpeed * Time.deltaTime;
		rx = Input.GetAxis("Horizontal2") * FrontTurnSpeed * Time.deltaTime;
		rz = Input.GetAxis("Vertical2") * FrontRunSpeed * Time.deltaTime;
		//Debug.Log ("fx = " + fx + "fz = " + fz + "rx =" + rx + "rz =" + rz);

	}

	void FixedUpdate(){
		//_front.AddForce (new Vector3 (fx, 0, fz), ForceMode.Impulse);
		//_rear.AddForce (new Vector3 (rx, 0, rz));
		_front.AddForceAtPosition(transform.forward * fz, _frontPivot.position, ForceMode.Impulse);
		_front.AddForceAtPosition(transform.right * fx, _frontPivot.position, ForceMode.Impulse);
		_front.AddForceAtPosition(transform.forward * rz, _rearPivot.position, ForceMode.Impulse);
		_front.AddForceAtPosition(transform.right * rx, _rearPivot.position, ForceMode.Impulse);
	}

	void LateUpdate()
	{
		_camera.position = transform.position + new Vector3(0,20,0);
	}
}