using UnityEngine;
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
	private float fx, fz, rx, rz, y;
	private Vector3 pVector;

	void Start()
	{
		_camera = transform.GetChild (0);	
		_camera.position = transform.position + new Vector3(0,20,0);
		_camera.LookAt(transform);
		_frontPivot = transform.GetChild(2);
		_rearPivot = transform.GetChild(3);
		pVector = new Vector3(0,0,0);
	}

	void Update()
	{
		//if (!isLocalPlayer)
		//{
	//		return;
	//	}
	
		fx = Input.GetAxis("Horizontal") * Time.deltaTime * FrontTurnSpeed;
		fz = Input.GetAxis("Vertical") * Time.deltaTime * FrontRunSpeed;
		rx = Input.GetAxis("Horizontal2") * Time.deltaTime * FrontTurnSpeed;
		rz = Input.GetAxis("Vertical2") * Time.deltaTime * FrontRunSpeed;
		pVector = Vector3.Project(Vector3.forward, pVector);
		pVector /= 4;
		pVector += new Vector3(0, 0, fz + rz);
		transform.Translate(pVector);
		transform.RotateAround(_frontPivot.position, Vector3.up, fx);
	}

	void LateUpdate()
	{
		transform.RotateAround(_rearPivot.position, Vector3.up, rx);
		_camera.position = transform.position + new Vector3(0,20,0);
	}
}