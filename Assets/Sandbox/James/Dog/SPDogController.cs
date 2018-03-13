using UnityEngine;
using UnityEngine.Networking;

public class SPDogController : MonoBehaviour //NetworkBehaviour
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
		_camera = Camera.allCameras[0].transform;
		_camera.SetParent(transform);
		_camera.position = transform.position + new Vector3(0,20,0);
		_camera.LookAt(transform);
		_frontPivot = transform.Find("frontPivot");
		_rearPivot = transform.Find("rearPivot");
		_front = GetComponent<Rigidbody> ();
	}

	void Update()
	{
	
		fx = Input.GetAxis("Horizontal") * FrontTurnSpeed * Time.deltaTime;
		fz = Input.GetAxis("Vertical") * FrontRunSpeed * Time.deltaTime;
		rx = Input.GetAxis("Horizontal2") * FrontTurnSpeed * Time.deltaTime;
		rz = Input.GetAxis("Vertical2") * FrontRunSpeed * Time.deltaTime;
		//Debug.Log ("fx = " + fx + "fz = " + fz + "rx =" + rx + "rz =" + rz);

	}

	void FixedUpdate(){
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