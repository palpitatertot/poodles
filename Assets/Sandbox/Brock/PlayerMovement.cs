using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public Rigidbody rb;
	public float moveSpeed;
	public float turnSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		


		if (Input.GetKey("d")) {
			transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
	}
		if (Input.GetKey("a")) {
			transform.Translate(-Vector3.right * moveSpeed * Time.deltaTime);
		}
		if (Input.GetKey("w")) {
			transform.Translate (-Vector3.up * moveSpeed * Time.deltaTime);
		}
		if (Input.GetKey("s")) {
			transform.Translate (Vector3.up * moveSpeed * Time.deltaTime);
		}
		if (Input.GetKeyDown (KeyCode.Space)) {
			

		}

		if(Input.GetKey(KeyCode.LeftArrow))
			transform.Rotate(0, -turnSpeed * Time.deltaTime, 0, Space.World);

		if(Input.GetKey(KeyCode.RightArrow))
			transform.Rotate(0, turnSpeed * Time.deltaTime, 0, Space.World);






}
			}
