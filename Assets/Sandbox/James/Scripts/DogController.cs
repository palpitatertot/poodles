using UnityEngine;
using UnityEngine.Networking;

public class DogController : NetworkBehaviour
{
    public ParticleSystem FrontPeePoofs;
    public ParticleSystem RearPeePoofs;
	//public float FrontRunSpeed;
	//public float RearRunSpeed;
	//public float FrontTurnSpeed;
	//public float RearTurnSpeed;
	//public float MouseSpeed;

    private bool isPenned = false;

	private Transform _frontPivot;
	private Transform _rearPivot;
	private Rigidbody _front;
    //private Rigidbody _rear;
	//private float fx, fz, rx, rz, y;
    Vector4 _splatChannel = new Vector4(1, 0, 0, 0);
    private Splatter _emitter;
    private Animator _animator;

	private InputHandler _inputH;
	private dogInput _inputD;

    private AudioSource[] _sfx;
	private float _runStartVolume;
    private Drinker d;

	void Start()
	{
        _frontPivot = transform.Find("frontPivot");
        _rearPivot = transform.Find("rearPivot");
        FrontPeePoofs.transform.position = _frontPivot.transform.position;
        RearPeePoofs.transform.position = _rearPivot.transform.position;
        _front = GetComponent<Rigidbody>();
        _animator = this.GetComponentInChildren<Animator>();
		_inputH = GetComponent<InputHandler>();
        _sfx = GetComponents<AudioSource>();
		_runStartVolume = _sfx [2].volume;
        d = GetComponent<Drinker>();

        if (isLocalPlayer)
        {
           
            _emitter = GetComponent<Splatter>();
            _emitter.SetEmitter(transform.Find("rearPivot"));
            _emitter.SetChannel(_splatChannel);

        }
	}

	void Update()
	{
        if (!isLocalPlayer){
            return;    
        }

        if(isPenned){
            SetPenned(false);
            gameObject.GetComponent<SpawnController>().BeginRespawn();
        }

        _animator.SetFloat("Velocity", _front.velocity.magnitude);
		if (!isLocalPlayer) {
			return;
		}
		_inputD = _inputH.getInputData ();


		if (Input.GetKey (KeyCode.F)) { //bark
			PlaySound (_sfx [0]);
		}

		if (_inputD.splat) {
			_emitter.Splat ();
			if (!_sfx [4].isPlaying && d.WaterLevel > 0) {               
				_sfx [4].pitch = Random.Range (1.0f, 1.2f);
				_sfx [4].Play ();
			}
		} else {
			if (_sfx [4].isPlaying) {
				_sfx [4].Stop ();                
			}
		}


		if (_front.velocity.magnitude > 1) {
			//DoPeePoofs ();
			if (!_sfx [2].isPlaying) {
				_sfx [2].pitch = Random.Range (1.0f, 1.2f); //walking sounds
				_sfx[2].volume = _runStartVolume;
				_sfx [2].Play ();
			}
		} else {
			if (_sfx [2].isPlaying) {
				_sfx [2].volume = _runStartVolume * _front.velocity.magnitude;
				if(_sfx[2].volume <= 0.0f) _sfx [2].Stop ();
			}

			//StopPeePoofs ();
		}

		if (!Input.anyKey) {
			if (_sfx [2].isPlaying) _sfx [2].Stop ();
		}
	}

	void FixedUpdate(){        
		_front.AddForceAtPosition(transform.forward * _inputD.fz, _frontPivot.position, ForceMode.Impulse);
		_front.AddForceAtPosition(transform.right * _inputD.fx, _frontPivot.position, ForceMode.Impulse);
		_front.AddForceAtPosition(transform.forward * _inputD.rz, _rearPivot.position, ForceMode.Impulse);
		_front.AddForceAtPosition(transform.right * _inputD.rx, _rearPivot.position, ForceMode.Impulse);

        if(!_inputH.isHeld()) // moves dog to floor
        {
            transform.position = new Vector3(transform.position.x, 4f, transform.position.z);
        }
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
        if(isServer){
            _moveDog(moveLocation);
            RpcMoveDog(moveLocation);
        }
        else {
            CmdMoveDog(moveLocation);
            RpcMoveDog(moveLocation);
        }
	}

    void _moveDog(Vector3 moveLocation)
    {
        transform.position = moveLocation;
        transform.rotation = Quaternion.identity;
    }

    [ClientRpc]
    void RpcMoveDog(Vector3 moveLocation){
        _moveDog(moveLocation);
    }

    [Command]
    void CmdMoveDog(Vector3 moveLocation){
        _moveDog(moveLocation);
        RpcMoveDog(moveLocation);
    }

    public void SetPenned(bool p){
        if(isServer){
            isPenned = p;
            RpcSetPenned(p);
        } else {
            CmdSetPenned(p);
        }
    }

    [Command]
    void CmdSetPenned(bool p){
        isPenned = p;
        RpcSetPenned(p);
    }

    [ClientRpc]
    void RpcSetPenned(bool p){
        isPenned = p;
    }
}