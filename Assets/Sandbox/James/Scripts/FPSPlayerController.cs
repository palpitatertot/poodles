using UnityEngine;
using UnityEngine.Networking;

public class FPSPlayerController : NetworkBehaviour {
	public float PlayerRunSpeed;
    public float PlayerTurnSpeed = 1f;
	public float MouseSpeed;
	public float CameraPitchClamp;
	public GameObject TPMesh;
	public GameObject RightArm;
    public GameObject LeftArm;

    private Animator _animator;
    private bool _mopping = false;
    private Quaternion _leftGrabRotation;
    private Quaternion _rightGrabRotation;
    private Quaternion _leftDownRotation;
    private Quaternion _rightDownRotation;
    private CharacterController _front;
    private Rigidbody _body;
	private float _pitch;
	private Transform _camera;
	private Transform _mopBottom;
	Vector4 _splatChannel = new Vector4(0, 0, 0, 0);
	private Splatter _emitter;
    private Vector3 _motion;
    private Vector3 _rotation;
    private AudioSource[] _sfx;
    private Grabber g;

    void Start()
	{
        _motion = Vector3.zero;
        _rotation = Vector3.zero;
		_pitch = 0f;
        if (isLocalPlayer)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _sfx = GetComponents<AudioSource>();
            g = GetComponent<Grabber>();
            _camera = Camera.allCameras[0].transform;
            _camera.position = transform.position - new Vector3(0, -7f, .5f);
            _camera.gameObject.GetComponent<Camera>().cullingMask &= ~(1 << LayerMask.NameToLayer("CameraInvisible"));
            _camera.SetParent(transform);
			_emitter = GetComponent<Splatter>();
			_emitter.SetEmitter(transform.GetChild(2).transform);
			_emitter.SetChannel(_splatChannel);
            //_body = GetComponent<Rigidbody>();
            _front = GetComponent<CharacterController>();
			TPMesh.SetActive (false);
			LeftArm.SetActive (true);
            RightArm.SetActive(true);

            /*_rightGrabRotation = RightArm.transform.localRotation;
            _leftGrabRotation = LeftArm.transform.localRotation;
            LeftArm.transform.Rotate(new Vector3(90, 0, 0));
            RightArm.transform.Rotate(new Vector3(90, 0, 0));
            _rightDownRotation = RightArm.transform.localRotation;
            _leftDownRotation = LeftArm.transform.localRotation;
*/
            _animator = LeftArm.GetComponentInChildren<Animator>();
        }
	}

	void Update()
	{   
		if (!isLocalPlayer)
		{
			return;
		}

        /*if(!g.hasObject){
            Grabbable gg = g.GetGrabbable();
            if (gg)
            {
                LeftArm.transform.localRotation = Quaternion.Lerp(_leftDownRotation, _leftGrabRotation, Time.time * .1f);
                RightArm.transform.localRotation = Quaternion.Lerp(_rightDownRotation, _rightGrabRotation, Time.time * .1f);
            }
            else
            {
                LeftArm.transform.localRotation = Quaternion.Lerp(_leftGrabRotation, _leftDownRotation, Time.time / .1f);
                RightArm.transform.localRotation = Quaternion.Lerp(_rightGrabRotation, _rightDownRotation, Time.time * .1f);
            }
        }*/


        if (Input.GetMouseButton(0))
        {
            _emitter.Splat();
            if (!_sfx[1].isPlaying && g.hasMop)
            {
                _sfx[1].pitch = Random.Range(1.0f, 1.2f);
                _sfx[1].Play();
            }
            if (!_mopping && g.hasMop)
            {
                Debug.Log("Now Mopping");
                _mopping = true;
                _animator.SetBool("isMopping", true);
                //LeftArm.transform.localRotation = _leftDownRotation;
            }
        }
        else
        {
            if (_sfx[1].isPlaying)
            {
                _sfx[1].Stop();
            }
            if (_mopping)
            {
                Debug.Log("Not Mopping");
                _mopping = false;
                //LeftArm.transform.localRotation = _leftGrabRotation;
                _animator.SetBool("isMopping", false);
            }
        }
        var x = Input.GetAxis("Horizontal") * transform.right;
		var z = Input.GetAxis("Vertical") * transform.forward;
        _motion = (x + z).normalized * PlayerRunSpeed * Time.deltaTime;
		var rot = Input.GetAxis("Mouse X") * MouseSpeed;
        _rotation = new Vector3(0, rot, 0) * PlayerTurnSpeed;
		_pitch -= Input.GetAxis("Mouse Y") * MouseSpeed;
		_pitch = Mathf.Clamp (_pitch, -CameraPitchClamp, CameraPitchClamp);
        transform.Rotate(_rotation);
        _front.Move(_motion);
        _camera.localRotation = Quaternion.AngleAxis (_pitch, Vector3.right);
        if(_front.velocity.magnitude > 1)
        {
            if (!_sfx[0].isPlaying)
            {
                _sfx[0].pitch = Random.Range(1.0f, 1.2f); //walking sounds                
                _sfx[0].Play();
            }
        }
        else
        {
            if (_sfx[0].isPlaying)
            {
                _sfx[0].Stop();
            }
        }
	}

	private void FixedUpdate()
	{
        //if(_motion != Vector3.zero){
        //    _body.MovePosition(_body.position + _motion * PlayerRunSpeed * Time.fixedDeltaTime);    
        //}

        //_body.MoveRotation(_body.rotation * Quaternion.Euler(_rotation));
	}



}