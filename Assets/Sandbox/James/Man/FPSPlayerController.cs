using UnityEngine;
using UnityEngine.Networking;

public class FPSPlayerController : NetworkBehaviour
{
	public float PlayerRunSpeed;
	public float MouseSpeed;
	public float CameraPitchClamp;

	private float _pitch;
	private Transform _camera;

	void Start()
	{
		_pitch = 0f;
		_camera = transform.GetChild (0);	
		_camera.gameObject.GetComponent<Camera> ().cullingMask &= ~(1 << LayerMask.NameToLayer("CameraInvisible"));
	}

	void Update()
	{
		if (!isLocalPlayer)
		{
			return;
		}

		var x = Input.GetAxis("Horizontal") * Time.deltaTime * PlayerRunSpeed;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * PlayerRunSpeed;
		var rotation = Input.GetAxis("Mouse X") * MouseSpeed;
		_pitch -= Input.GetAxis("Mouse Y") * MouseSpeed;
		_pitch = Mathf.Clamp (_pitch, -CameraPitchClamp, CameraPitchClamp);
		transform.Translate(x, 0, z);
		transform.Rotate(0, rotation, 0);
		_camera.localRotation = Quaternion.AngleAxis (_pitch, Vector3.right);
	}

	void LateUpdate()
	{
		
	}
}