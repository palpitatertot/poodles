using UnityEngine;
using UnityEngine.Networking;

public class DogController : NetworkBehaviour
{
    public ParticleSystem FrontPeePoofs;
    public ParticleSystem RearPeePoofs;
	public float FrontRunSpeed;
	public float RearRunSpeed;
	public float FrontTurnSpeed;
	public float RearTurnSpeed;
	public float MouseSpeed;
    public Vector3 CameraHeight = new Vector3(0, 35, 0);
	private Transform _camera;
	private Transform _frontPivot;
	private Transform _rearPivot;
	private Rigidbody _front;
    private Rigidbody _rear;
	private float fx, fz, rx, rz, y;
    Vector4 _splatChannel = new Vector4(1, 0, 0, 0);
    private Splatter _emitter;
    private Animator _animator;

	void Start()
	{
        _frontPivot = transform.Find("frontPivot");
        _rearPivot = transform.Find("rearPivot");
        FrontPeePoofs.transform.position = _frontPivot.transform.position;
        RearPeePoofs.transform.position = _rearPivot.transform.position;
        _front = GetComponent<Rigidbody>();
        //_animator = this.GetComponentInChildren<Animator>();


        if (isLocalPlayer)
        {
            _camera = Camera.allCameras[0].transform;
            _camera.SetParent(transform);
            _camera.position = transform.position + CameraHeight;
            _camera.LookAt(transform);
            _emitter = GetComponent<Splatter>();
            _emitter.SetEmitter(transform.Find("rearPivot"));
            _emitter.SetChannel(_splatChannel);
        }
	}

	void Update()
	{
		if (!isLocalPlayer)
		{
			return;
		}
		fx = Input.GetAxis("Horizontal") * FrontTurnSpeed * Time.deltaTime;
		fz = Input.GetAxis("Vertical") * FrontRunSpeed * Time.deltaTime;
		rx = Input.GetAxis("Horizontal2") * FrontTurnSpeed * Time.deltaTime;
		rz = Input.GetAxis("Vertical2") * FrontRunSpeed * Time.deltaTime;
        //_animator.SetFloat("Velocity", _front.velocity.magnitude);
        //Debug.Log(_animator.GetFloat("Velocity"));
		if (Input.GetKey(KeyCode.Space))
		{
			_emitter.Splat();
		}
        if (_front.velocity.magnitude > 1){
            DoPeePoofs();
        } else {
            StopPeePoofs();
        }
	}

	void FixedUpdate(){        
		_front.AddForceAtPosition(transform.forward * fz, _frontPivot.position, ForceMode.Impulse);
		_front.AddForceAtPosition(transform.right * fx, _frontPivot.position, ForceMode.Impulse);
		_front.AddForceAtPosition(transform.forward * rz, _rearPivot.position, ForceMode.Impulse);
		_front.AddForceAtPosition(transform.right * rx, _rearPivot.position, ForceMode.Impulse);

	}

	void LateUpdate()
	{
        if(!isLocalPlayer)
        {
            return;
        }
		_camera.position = transform.position + CameraHeight;
	}

    void DoPeePoofs()
    {
        var main = FrontPeePoofs.main;
        main.startColor = new Color(SplatColor.CYAN.x, SplatColor.CYAN.y, SplatColor.CYAN.z );
        FrontPeePoofs.Play(); // Continue normal emissions
        main = RearPeePoofs.main;
        main.startColor = new Color(SplatColor.CYAN.x, SplatColor.CYAN.y, SplatColor.CYAN.z);
        RearPeePoofs.Play(); // Continue normal emissions
    }
    void StopPeePoofs()
    {
        RearPeePoofs.Stop();   
    }
}