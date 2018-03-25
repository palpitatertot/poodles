using UnityEngine;
using UnityEngine.Networking;

public class FPSPlayerController : NetworkBehaviour {
	public float PlayerRunSpeed;
	public float MouseSpeed;
	public float CameraPitchClamp;
	public GameObject TPMesh;
	public GameObject FPMesh;
    private CharacterController _front; 
	private float _pitch;
	private Transform _camera;
	Vector4 _splatChannel = new Vector4(0, 0, 0, 0);
	private Splatter _emitter;
    private Vector3 _motion;

	void Start()
	{
		_pitch = 0f;
        if (isLocalPlayer)
        {
            _camera = Camera.allCameras[0].transform;
            _camera.position = transform.position - new Vector3(0, -0.75f, .5f);
            _camera.gameObject.GetComponent<Camera>().cullingMask &= ~(1 << LayerMask.NameToLayer("CameraInvisible"));
            _camera.SetParent(transform);
			_emitter = GetComponent<Splatter>();
			_emitter.SetEmitter(transform);
			_emitter.SetChannel(_splatChannel);
            _front = GetComponent<CharacterController>();
			TPMesh.SetActive (false);
			FPMesh.SetActive (true);

        }
		
	}

	void Update()
	{
		if (!isLocalPlayer)
		{
			return;
		}

		if (Input.GetMouseButton(0))
		{
			_emitter.Splat();
		}
        _motion = Vector3.zero;
		var x = Input.GetAxis("Horizontal") * Time.deltaTime * PlayerRunSpeed;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * PlayerRunSpeed;
		var rotation = Input.GetAxis("Mouse X") * MouseSpeed;
		_pitch -= Input.GetAxis("Mouse Y") * MouseSpeed;
		_pitch = Mathf.Clamp (_pitch, -CameraPitchClamp, CameraPitchClamp);
        //transform.Translate(x, 0, z);
        transform.Rotate(0, rotation, 0);
        _motion = new Vector3(x, 0, z);
        _motion = transform.TransformDirection(_motion);
        _front.Move(_motion);
        _camera.localRotation = Quaternion.AngleAxis (_pitch, Vector3.right);
	}

		

    
}