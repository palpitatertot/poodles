using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : MonoBehaviour {
	private int MESH = 1, LIGHT_SABER = 2, BULLET_SPAWN = 3, CAMERA = 0;	

	public float GravityMod = 1.2f;
	public float PlayerRotSpeed = 500.0f;
	public float PlayerMoveSpeed = 10.0f;
	public float JumpHeight = 20.0f;
	public float GroundDistance = .6f;
	public float SaberSpeed = 5.0f;	
	public Transform AimPoint;

	private bool _running;
	private Transform _shield;
	private bool _invincible;
	private bool _crouch;
	private Transform _mesh;
	private Vector3 _meshDefaultScale;
	private Vector3 _cameraOffset;
	private LayerMask _groundLayer;
	private Vector3 _velocity;
	private float _gravity;
	private bool _isGrounded;
	private Transform _groundCheck;	
	private CharacterController _controller;

	//private Transform _bulletSpawn;


	// Use this for initialization
	void Start () {
		//_invincible = false;
		//_mesh = transform.GetChild (MESH);
		//_meshDefaultScale = _mesh.localScale;
		//_gravity = Physics.gravity.y * GravityMod;
		//_controller = GetComponent<CharacterController>();
		//_groundLayer = 1<<LayerMask.NameToLayer("Ground") | 1<<LayerMask.NameToLayer("Default") & ~(1<<LayerMask.NameToLayer("Player"));		
	}
	
	// Update is called once per frame
	void Update() {

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);
        //_isGrounded = Physics.CheckSphere(transform.position, GroundDistance, _groundLayer, QueryTriggerInteraction.Ignore);
        //_mesh.localScale = new Vector3 (_meshDefaultScale.x, _meshDefaultScale.y, _meshDefaultScale.z);
        //_mesh.localPosition = new Vector3 (0, 0, 0);

        //float deltaRotate = Input.GetAxis ("Mouse X") * PlayerRotSpeed;
        //transform.Rotate (0, deltaRotate, 0);

        //float moveSpeed = PlayerMoveSpeed;
        //if (_isGrounded && Input.GetKey (KeyCode.LeftShift)) {
        //	_running = true;
        //	moveSpeed = 2 * PlayerMoveSpeed;
        //} else {
        //	_running = false;
        //}
        //Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * moveSpeed;
        //move = transform.rotation * move;

        //if (Input.GetKeyDown(KeyCode.Space) && _isGrounded) {
        //	_velocity.y += Mathf.Sqrt (JumpHeight * -2f * _gravity);
        //}
        //_velocity.y += _gravity * Time.deltaTime;
        //if (_isGrounded && _velocity.y < 0) {
        //	_velocity.y = 0f;
        //}
        //move.y = _velocity.y;

        //_controller.Move(move * Time.deltaTime);

        //if(Input.GetMouseButtonDown(0) ){	
        //}       			
    }


		
}