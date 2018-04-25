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


	private Transform _frontPivot;
	private Transform _rearPivot;
	private Rigidbody _front;
    private Rigidbody _rear;
	private float fx, fz, rx, rz, y;
    Vector4 _splatChannel = new Vector4(1, 0, 0, 0);
    private Splatter _emitter;
    private Animator _animator;

	private InputHandler _inputH;
	private dogInput _inputD;

    private AudioSource[] _sfx;

	void Start()
	{
        _frontPivot = transform.Find("frontPivot");
        _rearPivot = transform.Find("rearPivot");
        FrontPeePoofs.transform.position = _frontPivot.transform.position;
        RearPeePoofs.transform.position = _rearPivot.transform.position;
        _front = GetComponent<Rigidbody>();
        //_animator = this.GetComponentInChildren<Animator>();
		_inputH = GetComponent<InputHandler>();
        _sfx = GetComponents<AudioSource>(); 


        if (isLocalPlayer)
        {
           
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

		_inputD = _inputH.getInputData ();

        if (Input.GetKey(KeyCode.F)) //bark
        {
            PlaySound(_sfx[0]);
        }

		//fx = Input.GetAxis("Horizontal") * FrontTurnSpeed * Time.deltaTime;
		//fz = Input.GetAxis("Vertical") * FrontRunSpeed * Time.deltaTime;
		//rx = Input.GetAxis("Horizontal2") * FrontTurnSpeed * Time.deltaTime;
		//rz = Input.GetAxis("Vertical2") * FrontRunSpeed * Time.deltaTime;
        //_animator.SetFloat("Velocity", _front.velocity.magnitude);
        //Debug.Log(_animator.GetFloat("Velocity"));
		//if (Input.GetKey(KeyCode.Space))
		//{
		//	_emitter.Splat();
		//}

		if (_inputD.splat)
        {
			_emitter.Splat ();
            if (!_sfx[4].isPlaying)
            {
                _sfx[4].pitch = Random.Range(1.0f, 1.2f);
                _sfx[4].Play();
            }
        }
        else
        {
            if (_sfx[4].isPlaying)
            {
                _sfx[4].Stop();                
            }
        }

        if (_front.velocity.magnitude > 1)
        {
            DoPeePoofs();
            if (!_sfx[2].isPlaying)
            {
                _sfx[2].pitch = Random.Range(1.0f, 1.2f); //walking sounds
                _sfx[2].Play();
            }
        }
        else
        {
            if (_sfx[2].isPlaying)
            {
                _sfx[2].Stop();
            }

            StopPeePoofs();
        }

        if (!Input.anyKey)
        {
            _sfx[2].Stop();
        }
	}

	void FixedUpdate(){        
		_front.AddForceAtPosition(transform.forward * _inputD.fz, _frontPivot.position, ForceMode.Impulse);
		_front.AddForceAtPosition(transform.right * _inputD.fx, _frontPivot.position, ForceMode.Impulse);
		_front.AddForceAtPosition(transform.forward * _inputD.rz, _rearPivot.position, ForceMode.Impulse);
		_front.AddForceAtPosition(transform.right * _inputD.rx, _rearPivot.position, ForceMode.Impulse);

	}

    /*void LateUpdate()
	{
        if(!isLocalPlayer)
        {
            return;
        }
	}*/

    private void PlaySound(AudioSource source)
    {
        source.pitch = Random.Range(1.0f, 1.2f);
        source.Play();
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
		
	public
	void moveDog(Vector3 moveLocation)
	{
		if(isServer) transform.position = moveLocation;
	}
		
}