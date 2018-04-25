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


	void Start()
	{
        _frontPivot = transform.Find("frontPivot");
        _rearPivot = transform.Find("rearPivot");
        FrontPeePoofs.transform.position = _frontPivot.transform.position;
        RearPeePoofs.transform.position = _rearPivot.transform.position;
        _front = GetComponent<Rigidbody>();
        //_animator = this.GetComponentInChildren<Animator>();
		_inputH = GetComponent<InputHandler>();


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

		if (_inputD.splat)
			_emitter.Splat ();

        if (_front.velocity.magnitude > 1){
            //DoPeePoofs();
        } else {
            //StopPeePoofs();
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
}